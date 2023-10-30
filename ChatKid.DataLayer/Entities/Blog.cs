using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.DataLayer.Entities
{
    public partial class Blog : IBaseEntity, ISoftDelete
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public string? VoiceUrl { get; set; }

        public short? Status { get; set; }

        public Guid? TypeBlogId { get; set; }

        public virtual TypeBlog? TypeBlog { get; set; }

        public virtual Admin? CreateAdmin { get; set; }

        public virtual Admin? UpdateAdmin { get; set; }
    }
}
