using ChatKid.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Repositories
{
    public class KidServiceRepository : EfRepository<KidService>, IKidServiceRepository
    {
        public KidServiceRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
