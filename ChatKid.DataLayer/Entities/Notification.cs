using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Notification : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public string? Content { get; set; }
    
    public Guid? CreatedBy { get; set; }

    public string? Receiver { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public short? Status { get; set; }

    public virtual Admin? CreateAdmin { get; set; }

}
