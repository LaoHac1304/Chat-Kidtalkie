using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class PaymentMethodRepository : EfRepository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
