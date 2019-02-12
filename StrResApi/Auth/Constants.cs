namespace StrResApi.Auth
{
    public class Constants
    {
        public const string ADMIN_NAME_HEADER = "admin-name";
        public const string TENANT_ID_HEADER = "tenant-id";
        public const string ACCESS_TOKEN_HEADER = "access-token";
        public const string ISSUER = "StrResApi";
        public const string INVALID_REQUEST_MESSAGE = "Invalid request";
        public const string UNAUTHORIZED_ACCESS_MESSAGE = "Unauthorized access";
        public const string STR_RES_AUTH_SCHEME = "StrResAuthScheme";
        public const string STR_RES_AUTH_SCHEME_DISPLAY_NAME = "StrRes authentication scheme";
        public const string TENANT_ROLE = "Tenant";
        public const string ADMIN_ROLE = "Admin";
        public const string TENANT_ID_CLAIM_TYPE = "TenantId";
        public const string ADMIN_NAME_CLAIM_TYPE = "AdminName";
        public const long INVALID_TENANT_ID = -1;
    }
}