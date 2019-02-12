# Overview

Project StrRes implements a Web API for managing string resources.  The resources are scoped by tenant and the resources themselves are implemented as a key/value pair.

The main purpose of this project is to implement a cross-platform two-tier service that can serve both as a pattern for the creation of other services and be used when experimenting with various cloud build and deployment capabilities.

## Implementation Notes

* The service itself is implemented in C#/.NET Core using ASP.NET Core and Entity Framework (EF) Core.

* The schema design is via an EF Core code-first implementation.  The project currently support SQL Server and SQLite.  (See configuration sections below.)

* Even though it's arguably overkill, the Web API is implemented using the services and repository patterns. (<https://www.forevolve.com/en/articles/2017/08/11/design-patterns-web-api-service-and-repository-part-1/>)

## Security Notes

The API is secured as follows.

### Admins

* Only admins can CRUD tenants.

* Admins can do anything and read anything without regards to security.

* There is no API for creating or modifying admins.  This is intentional.  Admin data must be created outside the application.

### Tenants

* Tenants can modify info for their own tenant.

* Tenants can CRUD resources within their own tenant.

### Header Values

Every request requiring authorization should have one and only one of the following in the header:

* TenantId and AccessToken

* Admin name and AccessToken

See the StrResTest project for more guidance on calling the API.

Also note that the DateTime and Version controllers intentionally allow anonymous access.

## Solution Organization

The following projects make up the StrRes solution.

* StrResApi - the Web API itself implemented in ASP.NET Core

* StrResApiLib - a simple client library that can be used to call StrResApi.

* StrResConfiguration - a class to assist with the runtime configuration of StrResApi.

* StrResData - implements the data repositories, interfaces and entities.

* StrResServices - implements the service layer.

* StrResTest - implements a suite of end-to-end tests.  These are not unit tests and as such require a functioning application deployment.

## Creating the Database

The database can be created via the __dotnet ef__ command.  Before beginning, be sure to set the environment variables as described in the _Running the Application_ section.

To create a EF migration, execute a command similar to the following:

    dotnet ef migrations add dbv1

To create a script that can be used to create the database, execute the following command:

    dotnet ef migrations script

To create/update the database, use the following command:

    dotnet ef database update

## Running the Application

To run the application the following environment variables must be defined:

* STRRES_DB_PLATFORM: set to either "SQL_SERVER" or "SQLITE"

* STRRES_SQL_SERVER_DB_CONNECTION_STRING: the connection string for SQL Server.  Required if STRRES_DB_PLATFORM set to "SQL_SERVER"

* STRRES_SQLITE_DB_CONNECTION_STRING: the connection string for SQLite.  Required if STRRES_DB_PLATFORM set to "SQLITE".

Note that in the StrResApi project, there are some example Bash and PowerShell scripts that can be used as a guide.

After setting the environment variables, execute the following command from the StrResApi project folder to build and run the application:

    dotnet run

## Running the Tests

Before running the tests, the following environment variables must be defined:

* STRRES_BASE_URL: The URL of the deployed API.  Ex: <https://strresapi.azurewebsites.net>

* STRRES_ADMIN_NAME: An admin name defined in the application database.

* STRRES_ADMIN_TOKEN: The admin's access token.

Note that the code defines defaults appropriate for local development.

Once the environment variables are properly defined, execute the following command from the StrResTest project folder:

    dotnet test

## Next

* Add Swagger support.

* Target Cosmos DB using the (upcoming) EF Core provider.

* Add some end to end tests that drive high volume.