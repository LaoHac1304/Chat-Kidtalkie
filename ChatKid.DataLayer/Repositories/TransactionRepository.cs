using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class TransactionRepository : EfRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
