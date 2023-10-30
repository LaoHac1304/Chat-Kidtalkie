using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.DataLayer.Repositories
{
    public class FamilyRepository : EfRepository<Family>, IFamilyRepository
    {
        public FamilyRepository(IDBContext dbContext) : base(dbContext)
        {
        }
        public async Task<Family> GetByEmailAsync(string email) => await base.TableNoTracking.Where(x => x.OwnerMail.Equals(email)).SingleOrDefaultAsync();
    }
}
