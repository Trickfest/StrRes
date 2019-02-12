using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;

namespace StrResServices.Interfaces
{
    public interface ITenantService
    {
        IEnumerable<Tenant> GetTenants();
        IEnumerable<Tenant> GetTenants(long tenantId);
        Task<Tenant> GetTenant(long id);
        Task AddTenant(ref Tenant tenant);
        Task UpdateTenant(Tenant tenant);
        Task DeleteTenant(Tenant tenant);
        Task<bool> VerifyTenant(string name, string accessToken);
    }
}
