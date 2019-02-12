using System;
using System.Collections.Generic;
using System.Text;
using StrResData.Entities;
using StrResData.Interfaces;

namespace StrResData.Repositories
{
    public class AdminRepository : EntityBaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(StrResDbContext context) : base(context) { }
    }
}