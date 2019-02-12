# example environment variable settings

[Environment]::SetEnvironmentVariable("STRRES_DB_PLATFORM", "SQL_SERVER", "Machine")
[Environment]::SetEnvironmentVariable("STRRES_SQL_SERVER_DB_CONNECTION_STRING", "Server=127.0.0.1,1433;uid=sa;pwd=<YourNewStrong!Passw0rd>;Database=StrResDb;Trusted_Connection=False;ConnectRetryCount=0", "Machine")

<#
Other Examples

[Environment]::SetEnvironmentVariable("STRRES_SQL_SERVER_DB_CONNECTION_STRING", "Server=(localdb)\\mssqllocaldb;Database=StrResDb;Trusted_Connection=True;ConnectRetryCount=0", "Machine")

[Environment]::SetEnvironmentVariable("STRRES_DB_PLATFORM", "SQLITE", "Machine")
[Environment]::SetEnvironmentVariable("STRRES_SQLITE_DB_CONNECTION_STRING", "Data Source=strres.db", "Machine")
#>
