using ChatKid.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Family? Family { get; set; }
        public virtual Expert? Expert { get; set; }
        public virtual Admin? Admin { get; set; }
    }
}
