using System;
using System.Collections.Generic;
using System.Text;
using StrResData.Entities;
using StrResData.Interfaces;

namespace StrResData.Repositories
{
    public class TenantRepository : EntityBaseRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(StrResDbContext context) : base(context) { }
    }
}