
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class WalletRepository : EfRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
