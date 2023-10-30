using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class KidService : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid? ChildrenId { get; set; }

    public Guid? ServiceId { get; set; }

    public virtual User Children { get; set; }

    public virtual ICollection<DiscussRoom> DiscussRooms { get; set; } = new List<DiscussRoom>();

    public virtual Service Service { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public short? Status { get; set; }
}
