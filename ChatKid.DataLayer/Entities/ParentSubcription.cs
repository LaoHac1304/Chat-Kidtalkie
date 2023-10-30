using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class ParentSubcription : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid? SubcriptionId { get; set; }

    public Guid? FamilyId { get; set; }

    public virtual ICollection<MoneyPayment> MoneyPayments { get; set; } = new List<MoneyPayment>();

    public virtual Family Family { get; set; }

    public virtual Subcription Subcription { get; set; }

    public short? Status { get; set; }
}
