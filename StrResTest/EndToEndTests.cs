using System;
using System.Collections.Generic;
using System.Threading;
using StrResApiLib;
using Xunit;
using static System.Environment;
using static System.Console;

namespace StrResTest
{
    public class EndToEndTests
    {
        private const string DEFAULT_BASE_URL = "https://localhost:10001";
        private const int DEFAULT_FETCH_DELAY_MS = 200;

        // the following need to be preloaded into the database
        private const string DEFAULT_PREDEFINED_ADMIN_NAME = "Mark";
        private const string DEFAULT_PREDEFINED_ADMIN_TOKEN = "123";
        private const long TENANT_ID_THAT_DOES_NOT_EXIST = 212234233321123; // it could exist but probably doesn't :-)
        private const string NO_ADMIN_NAME_SPECIFIED = null;
        private const long NO_TENANT_ID_SPECIFIED = -1;

        [Fact]
        public void VersionTest()
        {
            // merely check that the call succeeds
            Client client = new Client(GetBaseUrl());
            var r = client.GetVersion().Result;
            Assert.NotNull(r);
        }

        [Fact]
        public void DateTimeTest()
        {
            // merely check that the call succeeds
            Client client = new Client(GetBaseUrl());
            var r = client.GetDateTime().Result;
            Assert.NotNull(r);
        }

        [Fact]
        public void AuthInvalidRequestFailedTests()
        {
            Client client = new Client(GetBaseUrl());

            // test with no tenant id or admin name
            Assert.Throws<AggregateException>(() =>
            {
                Tenant tenant = client.PostTenant(GetTempName(), GetTempName()).Result;
            });

            // test with both tenant id and admin name
            Assert.Throws<AggregateException>(() =>
            {
                Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetTempName(), TENANT_ID_THAT_DOES_NOT_EXIST).Result;
            });

            // test with admin but no access token
            Assert.Throws<AggregateException>(() =>
            {
                Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetTempName()).Result;
            });

            // test with tenant id but no access token
            Assert.Throws<AggregateException>(() =>
            {
                Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), NO_ADMIN_NAME_SPECIFIED, new Random().Next()).Result;
            });
        }

        [Fact]
        public void AuthAdminFailedTests()
        {
            Client client = new Client(GetBaseUrl());

            // test with unknown admin name
            Assert.Throws<AggregateException>(() =>
            {
                Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetTempName(), NO_TENANT_ID_SPECIFIED, GetTempName()).Result;
            });

            // test with known admin name but invalid access token
            Assert.Throws<AggregateException>(() =>
            {
                Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetAdminName(), NO_TENANT_ID_SPECIFIED, GetTempName()).Result;
            });
        }

        [Fact]
        public void AuthTenantFailedTests()
        {
            Client client = new Client(GetBaseUrl());

            // create tenant
            Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken()).Result;

            // test with unknown tenant id
            Assert.Throws<AggregateException>(() =>
            {
                Tenant failedTenant = client.PostTenant(GetTempName(), GetTempName(), NO_ADMIN_NAME_SPECIFIED, TENANT_ID_THAT_DOES_NOT_EXIST, GetTempName()).Result;
            });

            // test with known tenant id but invalid token
            Assert.Throws<AggregateException>(() =>
            {
                Tenant failedTenant = client.PostTenant(GetTempName(), GetTempName(), NO_ADMIN_NAME_SPECIFIED, tenant.TenantId, GetTempName()).Result;
            });

            // delete tenant
            client.DeleteTenant(tenant, GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken());
        }

        [Fact]
        public void CrudTenant()
        {
            Client client = new Client(GetBaseUrl());

            // create tenant
            Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken()).Result;

            Tenant fetchedTenant = GetTenantWithDelay(client, tenant.TenantId, NO_ADMIN_NAME_SPECIFIED, tenant.TenantId, tenant.AccessToken);

            Assert.Equal(tenant.TenantId, fetchedTenant.TenantId);
            Assert.Equal(tenant.Name, fetchedTenant.Name);
            Assert.Equal(tenant.AccessToken, fetchedTenant.AccessToken);
            Assert.NotEqual(tenant.Name, fetchedTenant.AccessToken);
            Assert.NotEqual(tenant.AccessToken, fetchedTenant.Name);

            // try to create tenant again with same name
            Assert.Throws<AggregateException>(() =>
            {
                Tenant dupTenant = client.PostTenant(tenant.Name, GetTempName(), GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken()).Result;
            });

            // update tenant
            string currentTenantAccessToken = tenant.AccessToken;
            tenant.Name = GetTempName();
            tenant.AccessToken = GetTempName();

            client.UpdateTenant(tenant, NO_ADMIN_NAME_SPECIFIED, tenant.TenantId, currentTenantAccessToken);

            Tenant updatedTenant = GetTenantWithDelay(client, tenant.TenantId, NO_ADMIN_NAME_SPECIFIED, tenant.TenantId, tenant.AccessToken);

            Assert.Equal(tenant.TenantId, updatedTenant.TenantId);
            Assert.Equal(tenant.Name, updatedTenant.Name);
            Assert.Equal(tenant.AccessToken, updatedTenant.AccessToken);
            Assert.NotEqual(tenant.Name, updatedTenant.AccessToken);
            Assert.NotEqual(tenant.AccessToken, updatedTenant.Name);

            // get all tenants - make sure our new one is in the set
            {
                List<Tenant> tenants = GetTenantsWithDelay(client, GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken());

                bool found = false;
                foreach (Tenant t in tenants)
                {
                    if (t.TenantId == updatedTenant.TenantId)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.True(found);
            }

            // get just our tenant - make sure it is returned
            {
                List<Tenant> tenants = GetTenantsWithDelay(client, NO_ADMIN_NAME_SPECIFIED, updatedTenant.TenantId, updatedTenant.AccessToken);

                bool found = false;
                foreach (Tenant t in tenants)
                {
                    if (t.TenantId == updatedTenant.TenantId)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.True(found);

                // should only return one tenant
                Assert.True(tenants.Count == 1);
            }

            // delete the tenant we've created
            client.DeleteTenant(tenant, GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken());

            Assert.Throws<AggregateException>(() =>
            {
                Tenant deletedTenant = GetTenantWithDelay(client, tenant.TenantId, GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken());
            });
        }

        [Fact]
        public void CrudResource()
        {
            Client client = new Client(GetBaseUrl());

            // create tenant
            Tenant tenant = client.PostTenant(GetTempName(), GetTempName(), GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken()).Result;
            Tenant altTenant = client.PostTenant(GetTempName(), GetTempName(), GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken()).Result;

            // create resource
            Resource resource = client.PostResource(tenant.TenantId,
                GetTempName(),
                GetTempName(),
                NO_ADMIN_NAME_SPECIFIED,
                tenant.TenantId,
                tenant.AccessToken).Result;

            // fetch resource
            Resource fetchedResource = GetResourceWithDelay(client,
                resource.TenantId,
                resource.Key,
                NO_ADMIN_NAME_SPECIFIED,
                tenant.TenantId,
                tenant.AccessToken);

            Assert.Equal(resource.TenantId, fetchedResource.TenantId);
            Assert.Equal(resource.Key, fetchedResource.Key);
            Assert.Equal(resource.Value, fetchedResource.Value);

            // create resource with same key but different tenant (should work)
            Resource altResource = client.PostResource(altTenant.TenantId,
                resource.Key,
                resource.Value,
                NO_ADMIN_NAME_SPECIFIED,
                altTenant.TenantId,
                altTenant.AccessToken).Result;

            // create resource again with same tenant id and key (should fail)
            Assert.Throws<AggregateException>(() =>
            {
                Resource badResource = client.PostResource(tenant.TenantId,
                resource.Key,
                GetTempName(),
                NO_ADMIN_NAME_SPECIFIED,
                tenant.TenantId,
                tenant.AccessToken).Result;
            });

            // update resource
            resource.Value = GetTempName();
            client.UpdateResource(resource, NO_ADMIN_NAME_SPECIFIED, tenant.TenantId, tenant.AccessToken);

            Resource updatedResource = GetResourceWithDelay(client,
                resource.TenantId,
                resource.Key,
                NO_ADMIN_NAME_SPECIFIED,
                tenant.TenantId,
                tenant.AccessToken);

            Assert.Equal(resource.TenantId, updatedResource.TenantId);
            Assert.Equal(resource.Key, updatedResource.Key);
            Assert.Equal(resource.Value, updatedResource.Value);

            {
                // get all resources system-wide - make sure our new one is in the set
                List<Resource> resources = GetResourcesWithDelay(client,
                    GetAdminName(),
                    NO_TENANT_ID_SPECIFIED,
                    GetAdminToken());

                bool found = false;
                foreach (Resource r in resources)
                {
                    if ((r.TenantId == updatedResource.TenantId) &&
                        (r.Key == updatedResource.Key) &&
                        (r.Value == updatedResource.Value))
                    {
                        found = true;
                        break;
                    }
                }

                Assert.True(found);
            }

            {
                // get all resources for tenant - make sure our new one is in the set
                List<Resource> resources = GetResourcesWithDelay(client,
                    NO_ADMIN_NAME_SPECIFIED,
                    tenant.TenantId,
                    tenant.AccessToken);

                bool found = false;
                foreach (Resource r in resources)
                {
                    if ((r.TenantId == updatedResource.TenantId) &&
                        (r.Key == updatedResource.Key) &&
                        (r.Value == updatedResource.Value))
                    {
                        found = true;
                        break;
                    }
                }

                Assert.True(found);
            }

            // delete the resources we've created
            client.DeleteResource(resource,
                NO_ADMIN_NAME_SPECIFIED,
                tenant.TenantId,
                tenant.AccessToken);

            client.DeleteResource(altResource,
                NO_ADMIN_NAME_SPECIFIED,
                altTenant.TenantId,
                altTenant.AccessToken);

            Assert.Throws<AggregateException>(() =>
            {
                Resource deletedResource = GetResourceWithDelay(client,
                    resource.TenantId,
                    resource.Key,
                    NO_ADMIN_NAME_SPECIFIED,
                    tenant.TenantId,
                    tenant.AccessToken);
            });

            Assert.Throws<AggregateException>(() =>
            {
                Resource deletedResource = GetResourceWithDelay(client,
                    altResource.TenantId,
                    altResource.Key,
                    NO_ADMIN_NAME_SPECIFIED,
                    altTenant.TenantId,
                    altTenant.AccessToken);
            });

            // delete the tenants we created for this test
            client.DeleteTenant(tenant, GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken());
            client.DeleteTenant(altTenant, GetAdminName(), NO_TENANT_ID_SPECIFIED, GetAdminToken());
        }

        public List<Tenant> GetTenantsWithDelay(Client client,
            string adminNameHeaderValue,
            long tenantIdHeaderValue,
            string accessTokenHeaderValue,
            int msDelay = DEFAULT_FETCH_DELAY_MS)
        {
            if (msDelay > 0)
            {
                Thread.Sleep(msDelay);
            }

            return client.GetTenants(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue).Result;
        }

        public Tenant GetTenantWithDelay(Client client,
            long tenantId,
            string adminNameHeaderValue,
            long tenantIdHeaderValue,
            string accessTokenHeaderValue,
            int msDelay = DEFAULT_FETCH_DELAY_MS)
        {
            if (msDelay > 0)
            {
                Thread.Sleep(msDelay);
            }

            return client.GetTenant(tenantId, adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue).Result;
        }

        public List<Resource> GetResourcesWithDelay(Client client,
            string adminNameHeaderValue,
            long tenantIdHeaderValue,
            string accessTokenHeaderValue,
            int msDelay = DEFAULT_FETCH_DELAY_MS
            )
        {
            if (msDelay > 0)
            {
                Thread.Sleep(msDelay);
            }

            return client.GetResources(adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue).Result;
        }

        public Resource GetResourceWithDelay(Client client,
            long tenantId,
            string key,
            string adminNameHeaderValue,
            long tenantIdHeaderValue,
            string accessTokenHeaderValue,
            int msDelay = DEFAULT_FETCH_DELAY_MS)
        {
            if (msDelay > 0)
            {
                Thread.Sleep(msDelay);
            }

            return client.GetResource(tenantId, key, adminNameHeaderValue, tenantIdHeaderValue, accessTokenHeaderValue).Result;
        }

        public string GetTempName()
        {
            return Guid.NewGuid().ToString();
        }

        public string GetBaseUrl()
        {
            string s = GetEnvironmentVariable("STRRES_BASE_URL");
            return string.IsNullOrWhiteSpace(s) ? DEFAULT_BASE_URL : s;
        }

        public string GetAdminName()
        {
            string s = GetEnvironmentVariable("STRRES_ADMIN_NAME");
            return string.IsNullOrWhiteSpace(s) ? DEFAULT_PREDEFINED_ADMIN_NAME : s;
        }

        public string GetAdminToken()
        {
            string s = GetEnvironmentVariable("STRRES_ADMIN_TOKEN");
            return string.IsNullOrWhiteSpace(s) ? DEFAULT_PREDEFINED_ADMIN_TOKEN : s;
        }
    }
}