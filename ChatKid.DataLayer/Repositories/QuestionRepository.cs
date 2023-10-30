using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Repositories
{
    public class QuestionRepository : EfRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IDBContext dbContext) : base(dbContext)
        {
        }
    }
}
