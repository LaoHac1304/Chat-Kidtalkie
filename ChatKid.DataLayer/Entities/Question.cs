using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Question : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? VoiceUrl { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedTime { get; set; }
    public string? Answer { get; set; }
    public string? Form { get; set; }   
    public Guid? KidServiceId { get; set; }

    public virtual KidService KidService { get; set; }
    public short? Status { get; set; }
}
