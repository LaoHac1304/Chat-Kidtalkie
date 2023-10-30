using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Service : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public short? Energy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual ICollection<KidService> KidServices { get; set; } = new List<KidService>();

    public short? Status { get; set; }
}
