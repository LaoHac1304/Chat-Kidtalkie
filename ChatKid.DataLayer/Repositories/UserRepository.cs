using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatKid.DataLayer.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(IDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsFullFamilyUser(Guid familyId) => base.TableNoTracking.Where(x => x.FamilyId.Equals(familyId)).Count() <= 5;

        public async Task<User> LoginWithPassword(Guid id, string password)
            => await base.TableNoTracking.Where(x => x.Id.Equals(id) && x.Password.Equals(password)).SingleOrDefaultAsync();
            
    }
}
