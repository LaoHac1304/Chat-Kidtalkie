using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatKid.DataLayer.Entities;

public partial class Expert : IBaseEntity, ISoftDelete
{
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gmail { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Phone { get; set; }

    public int? Age { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public virtual ICollection<DiscussRoom> DiscussRooms { get; set; } = new List<DiscussRoom>();
    public short? Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

}
