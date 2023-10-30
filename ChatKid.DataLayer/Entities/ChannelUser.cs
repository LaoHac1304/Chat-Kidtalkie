using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class ChannelUser : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid ChannelId { get; set; }

    public Guid UserId { get; set; }

    public string? NameInChannel { get; set; }

    public virtual Channel Channel { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User User { get; set; }

    public short? Status { get; set; }
}
