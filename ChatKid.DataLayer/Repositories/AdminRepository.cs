using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Repositories
{
    public class AdminRepository : EfRepository<Admin>, IAdminRepository
    {
        public AdminRepository(IDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            var admin = await TableNoTracking.FirstAsync(x => x.Gmail == email);
            return admin;
        }
    }
}
