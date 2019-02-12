using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;

namespace StrResServices.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<Admin> GetAdmins();
        Task<Admin> GetAdmin(string name);
        Task AddAdmin(ref Admin admin);
        Task UpdateAdmin(Admin admin);
        Task DeleteAdmin(Admin admin);
    }
}
