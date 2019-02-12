using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;

namespace StrResServices.Interfaces
{
    public interface IResourceService
    {
        IEnumerable<Resource> GetResources();        
        IEnumerable<Resource> GetResources(long tenantId);
        Task<Resource> GetResource(long tenantId, string key);
        Task AddResource(ref Resource resource);
        Task UpdateResource(Resource resource);
        Task DeleteResource(Resource resource);
    }
}
