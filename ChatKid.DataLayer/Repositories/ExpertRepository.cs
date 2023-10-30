using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Repositories
{
    public class ExpertRepository : EfRepository<Expert>, IExpertRepository
    {
        public ExpertRepository(IDBContext dbContext) : base(dbContext)
        {
            
        }

        public async  Task<Expert> GetExpertByGmailAsync(string gmail) => await TableNoTracking.FirstAsync(x => x.Gmail.Equals(gmail));
    }
}
