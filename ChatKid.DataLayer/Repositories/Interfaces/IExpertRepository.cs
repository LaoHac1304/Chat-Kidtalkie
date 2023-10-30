using ChatKid.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Repositories.Interfaces
{
    public interface IExpertRepository : IGenericRepository<Expert>
    {
        public Task<Expert> GetExpertByGmailAsync(string gmail);

    }
}
