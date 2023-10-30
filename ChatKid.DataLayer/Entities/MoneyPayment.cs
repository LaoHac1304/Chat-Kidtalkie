using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class MoneyPayment : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid? ParentSubcriptionId { get; set; }

    public Guid? DiscountId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedTime { get; set; }

    public short? Status { get; set; }

    public Guid? MethodId { get; set; }

    public virtual PaymentMethod Method { get; set; }

    public virtual ParentSubcription ParentSubcription { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
