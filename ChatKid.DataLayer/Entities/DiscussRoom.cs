using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class DiscussRoom : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid? KidServiceId { get; set; }

    public Guid? ExpertId { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual Expert Expert { get; set; }

    public virtual KidService KidService { get; set; }
    public string? VoiceUrl { get; set; }

    public short? Status { get; set; }
}
