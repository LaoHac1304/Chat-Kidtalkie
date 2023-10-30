using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class OtpRepository : EfRepository<Otp>, IOtpRepository
    {
        public OtpRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
