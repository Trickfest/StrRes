using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace StrResApiLib
{
    public class Client
    {
        private string _baseUrl;

        private const string VERSION_ROUTE = "api/version";
        private const string DATETIME_ROUTE = "api/datetime";
        private const string TENANT_ROUTE = "api/tenants";
        private const string RESOURCE_ROUTE = "api/resources";

        public Client(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Task<dynamic> GetVersion()
        {
            var result = _baseUrl
                .AppendPathSegment(VERSION_ROUTE).GetJsonAsync();

            return result;
        }

        public Task<dynamic> GetDateTime()
        {
            var result = _baseUrl
                .AppendPathSegment(DATETIME_ROUTE).GetJsonAsync();

            return result;
        }

        public Task<List<Tenant>> GetTenants(string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            var result = _baseUrl
                .AppendPathSegment(TENANT_ROUTE)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .GetJsonAsync<List<Tenant>>();

            return result;
        }

        public Task<Tenant> GetTenant(long tenantId, string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            var result = _baseUrl
                .AppendPathSegment(TENANT_ROUTE)
                .AppendPathSegment(tenantId)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .GetJsonAsync<Tenant>();

            return result;
        }

        public Task<Tenant> PostTenant(string tenantName,
            string accessToken,
            string adminNameHeaderValue = null,
            long tenantIdHeaderValue = -1,
            string accessTokenHeaderValue = null)
        {
            var tenant = _baseUrl
                .AppendPathSegment(TENANT_ROUTE)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .PostJsonAsync(new
                {
                    name = tenantName,
                    accessToken = accessToken
                })
                .ReceiveJson<Tenant>();

            return tenant;
        }

        public async void UpdateTenant(Tenant tenant,
            string adminNameHeaderValue = null,
            long tenantIdHeaderValue = -1,
            string accessTokenHeaderValue = null)
        {
            await _baseUrl
                .AppendPathSegment(TENANT_ROUTE)
                .AppendPathSegment(tenant.TenantId)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .PutJsonAsync(tenant);
        }

        public async void DeleteTenant(Tenant tenant,
            string adminNameHeaderValue = null,
            long tenantIdHeaderValue = -1,
            string accessTokenHeaderValue = null)
        {
            await _baseUrl
                .AppendPathSegment(TENANT_ROUTE)
                .AppendPathSegment(tenant.TenantId)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .DeleteAsync();
        }


        public Task<List<Resource>> GetResources(string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            var result = _baseUrl
                .AppendPathSegment(RESOURCE_ROUTE)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .GetJsonAsync<List<Resource>>();

            return result;
        }

        public Task<Resource> GetResource(long tenantId, string key, string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            var result = _baseUrl
                .AppendPathSegment(RESOURCE_ROUTE)
                .AppendPathSegment(tenantId)
                .AppendPathSegment(key)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .GetJsonAsync<Resource>();

            return result;
        }

        public Task<Resource> PostResource(long tenantId, string key, string value, string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            var tenant = _baseUrl
                .AppendPathSegment(RESOURCE_ROUTE)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .PostJsonAsync(new
                {
                    tenantId = tenantId,
                    key = key,
                    value = value
                })
                .ReceiveJson<Resource>();

            return tenant;
        }

        public async void UpdateResource(Resource resource, string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            await _baseUrl
                .AppendPathSegment(RESOURCE_ROUTE)
                .AppendPathSegment(resource.TenantId)
                .AppendPathSegment(resource.Key)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .PutJsonAsync(resource);
        }

        public async void DeleteResource(Resource resource, string adminNameHeaderValue = null, long tenantIdHeaderValue = -1, string accessTokenHeaderValue = null)
        {
            await _baseUrl
                .AppendPathSegment(RESOURCE_ROUTE)
                .AppendPathSegment(resource.TenantId)
                .AppendPathSegment(resource.Key)
                .SetStrResHeaders(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue)
                .DeleteAsync();
        }
    }

    public static class StrResFlurlUrlExtensions
    {
        private const string ADMIN_NAME_HEADER = "admin-name";
        private const string TENANT_ID_HEADER = "tenant-id";
        private const string ACCESS_TOKEN_HEADER = "access-token";

        public static IFlurlRequest SetStrResHeaders(this Url url,
            string adminNameHeaderValue,
            long tenantIdHeaderValue,
            string accessTokenHeaderValue)
        {
            // there may be a more straight-forward way to get the
            // IFlurlRequest object, but I couldn't find it
            var request = url.ConfigureRequest(settings => { });

            if (!string.IsNullOrWhiteSpace(adminNameHeaderValue))
            {
                request = request.WithHeader(ADMIN_NAME_HEADER, adminNameHeaderValue);
            }

            if (tenantIdHeaderValue > 0)
            {
                request = request.WithHeader(TENANT_ID_HEADER, tenantIdHeaderValue);
            }

            if (!string.IsNullOrWhiteSpace(accessTokenHeaderValue))
            {
                request = request.WithHeader(ACCESS_TOKEN_HEADER, accessTokenHeaderValue);
            }

            return request;
        }
    }
}