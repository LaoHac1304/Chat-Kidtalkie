using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class MoneyPaymentRepository : EfRepository<MoneyPayment>, IMoneyPaymentRepository
    {
        public MoneyPaymentRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
