using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatKid.DataLayer.Entities;

public partial class Family : IBaseEntity, ISoftDelete
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? OwnerMail { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public short? Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<ParentSubcription> ParentSubcriptions { get; set; } = new List<ParentSubcription>();
}
