using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;
using StrResData.Interfaces;
using StrResServices.Interfaces;

namespace StrResServices.Functional
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public IEnumerable<Resource> GetResources()
        {
            return _resourceRepository.GetAll();
        }

        public IEnumerable<Resource> GetResources(long tenantId)
        {
            return _resourceRepository.GetWhere(o => o.TenantId == tenantId);
        }

        public Task<Resource> GetResource(long tenantId, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Invalid resource key.", nameof(key));
            }

            return _resourceRepository.GetSingle(tenantId, key);
        }

        public Task AddResource(ref Resource resource)
        {
            return _resourceRepository.Add(ref resource);
        }

        public Task UpdateResource(Resource resource)
        {
            return _resourceRepository.Update(resource);
        }

        public Task DeleteResource(Resource resource)
        {
            return _resourceRepository.Delete(resource);
        }
    }
}