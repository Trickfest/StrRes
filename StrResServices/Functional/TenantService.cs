using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;
using StrResData.Interfaces;
using StrResServices.Interfaces;

namespace StrResServices.Functional
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public IEnumerable<Tenant> GetTenants(long tenantId)
        {
            return _tenantRepository.GetWhere(o => o.TenantId == tenantId);
        }

        public IEnumerable<Tenant> GetTenants()
        {
            return _tenantRepository.GetAll();
        }

        public Task<Tenant> GetTenant(long id)
        {
            return _tenantRepository.GetSingle(id);
        }

        public Task AddTenant(ref Tenant tenant)
        {
            return _tenantRepository.Add(ref tenant);
        }

        public Task UpdateTenant(Tenant tenant)
        {
            return _tenantRepository.Update(tenant);
        }

        public Task DeleteTenant(Tenant tenant)
        {
            return _tenantRepository.Delete(tenant);
        }
                
        public Task<bool> VerifyTenant(string name, string accessToken)
        {
            return Task.FromResult(true);
        }

    }
}