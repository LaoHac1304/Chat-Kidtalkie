using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Transaction : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public decimal? Price { get; set; }

    public short? Energy { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedTime { get; set; }

    public Guid? MoneyPaymentId { get; set; }

    public Guid? WalletId { get; set; }

    public Guid? KidServiceId { get; set; }

    public virtual KidService KidService { get; set; }

    public virtual MoneyPayment MoneyPayment { get; set; }

    public virtual Wallet Wallet { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public virtual User? CreateBy { get; set; }
    public short? Status { get; set; }
}
