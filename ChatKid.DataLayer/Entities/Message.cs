using ChatKid.DataLayer.EntityInterfaces;
using System;
using System.Collections.Generic;

namespace ChatKid.DataLayer.Entities;

public partial class Message : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public string? VoiceUrl { get; set; }

    public DateTime? SentTime { get; set; }

    public Guid? ChannelUserId { get; set; }

    public virtual ChannelUser ChannelUser { get; set; }
    public short? Status { get; set; }
}
