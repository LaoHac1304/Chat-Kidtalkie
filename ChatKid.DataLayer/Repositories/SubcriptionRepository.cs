using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class SubcriptionRepository : EfRepository<Subcription>, ISubcriptionRepository
    {
        public SubcriptionRepository(IDBContext dbContext) : base(dbContext)
        {
        }

    }
}
