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
    public class Notificationrepository : EfRepository<Notification>, INotificationRepository
    {
        public Notificationrepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
