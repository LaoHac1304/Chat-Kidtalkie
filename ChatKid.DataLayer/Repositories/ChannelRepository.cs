using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;

namespace ChatKid.DataLayer.Repositories
{
    public class ChannelRepository : EfRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
