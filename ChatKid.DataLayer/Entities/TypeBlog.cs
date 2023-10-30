using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Entities
{
    public class TypeBlog
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string? ImageUrl { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }

    }
}
