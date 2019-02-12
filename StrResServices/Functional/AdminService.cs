using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrResData.Entities;
using StrResData.Interfaces;
using StrResServices.Interfaces;

namespace StrResServices.Functional
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public IEnumerable<Admin> GetAdmins()
        {
            return _adminRepository.GetAll();
        }

        public Task<Admin> GetAdmin(string name)
        {
            return _adminRepository.GetSingle(name);
        }

        public Task AddAdmin(ref Admin admin)
        {
            return _adminRepository.Add(ref admin);
        }

        public Task UpdateAdmin(Admin admin)
        {
            return _adminRepository.Update(admin);
        }

        public Task DeleteAdmin(Admin admin)
        {
            return _adminRepository.Delete(admin);
        }
    }
}