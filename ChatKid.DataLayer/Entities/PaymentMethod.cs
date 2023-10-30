using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class PaymentMethod : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public short? Status { get; set; }

    public virtual ICollection<MoneyPayment> MoneyPayments { get; set; } = new List<MoneyPayment>();
}
