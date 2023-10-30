using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Wallet : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public short? TotalEnergy { get; set; }

    public Guid? OwnerId { get; set; }

    public short? Status { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual User Owner { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
