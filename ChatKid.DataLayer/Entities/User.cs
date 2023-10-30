using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatKid.DataLayer.Entities;

public partial class User : IBaseEntity, ISoftDelete
{

    public Guid Id { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public short? Status { get; set; }

    public Guid? FamilyId { get; set; }

    public string? DeviceToken { get; set; }

    public string? Gender { get; set; }

    public short? IsUpdated { get; set; }

    public virtual Family Family { get; set; }

    public virtual ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();

    public virtual ICollection<KidService> KidServices { get; set; } = new List<KidService>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
