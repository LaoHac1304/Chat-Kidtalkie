using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class ParentSubcriptionRepository : EfRepository<ParentSubcription>, IParentSubcriptionRepository
    {
        public ParentSubcriptionRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
