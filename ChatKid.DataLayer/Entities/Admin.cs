using ChatKid.DataLayer.EntityInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Entities
{
    public partial class Admin : IBaseEntity, ISoftDelete, ICreatedEntity, IUpdatedEntity
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gmail { get; set; }

        public string? Phone { get; set; }

        public string? AvatarUrl { get; set; }

        public int? Age { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }
        public short? Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public virtual ICollection<Blog> CreatedBlogs { get; set;}
        public virtual ICollection<Blog> UpdatedBlogs { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Advertising> Advertisings { get; set; }
    }
}
