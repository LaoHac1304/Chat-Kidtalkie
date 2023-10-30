using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.Models.RequestModels.TypeBlogRequests
{
    public class TypeBlogUpdateRequest
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
