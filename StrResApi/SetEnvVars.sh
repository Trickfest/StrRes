# example environment variable settings

STRRES_DB_PLATFORM="SQL_SERVER"
export STRRES_DB_PLATFORM

STRRES_SQL_SERVER_DB_CONNECTION_STRING="Server=127.0.0.1,1433;uid=sa;pwd=<YourNewStrong!Passw0rd>;Database=StrResDb;Trusted_Connection=False;ConnectRetryCount=0"
export STRRES_SQL_SERVER_DB_CONNECTION_STRING

STRRES_SQLITE_DB_CONNECTION_STRING="Data Source=strres.db"
export STRRES_SQLITE_DB_CONNECTION_STRING

#STRRES_DB_PLATFORM="SQLITE"