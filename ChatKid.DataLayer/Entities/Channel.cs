using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Channel : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();
    public short? Status { get; set; }
}
