using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;
using StrResData.Interfaces;

namespace StrResData.Repositories
{
    public class ResourceRepository : EntityBaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(StrResDbContext context) : base(context) { }
    }
}