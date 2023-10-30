using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Repositories
{
    public class TypeBlogRepository : EfRepository<TypeBlog>, ITypeBlogRepository
    {
        public TypeBlogRepository(IDBContext context) : base(context)
        {
        }
    }
   
}
