using System;
using static System.Environment;

namespace StrResConfiguration
{
    public static class StrResConfiguration
    {
        public enum DbPlatforms { SQL_SERVER, SQLITE };

        private const string ENV_VAR_PREFIX = "STRRES_";

        public static String DbConnectionString(DbPlatforms dbPlatform)
        {
            string s = dbPlatform.ToString();
            string connectionStringEnvironmentVariable = ENV_VAR_PREFIX + s + "_DB_CONNECTION_STRING";
            string v = GetEnvironmentVariable(connectionStringEnvironmentVariable);

            if (string.IsNullOrEmpty(v))
            {
                throw new Exception($"Environment variable {connectionStringEnvironmentVariable} not defined.");
            }

            return v;
        }

        public static DbPlatforms DbPlatform()
        {
            string dbPlatformEnvironmentVariable = ENV_VAR_PREFIX + "DB_PLATFORM";
            string dbPlatform = GetEnvironmentVariable(dbPlatformEnvironmentVariable);

            if (string.IsNullOrEmpty(dbPlatform))
            {
                throw new Exception($"Environment variable {dbPlatformEnvironmentVariable} not defined.");
            }

            return (DbPlatforms)Enum.Parse(typeof(DbPlatforms), dbPlatform);
        }
    }
}
