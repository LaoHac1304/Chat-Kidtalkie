using ChatKid.DataLayer.EntityInterfaces;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatKid.DataLayer.Entities;

public partial class Advertising : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }
    public string? Title { get; set;}
    public string? Content { get; set; }
    public string? Company { get; set; }
    public string? CompanyEmail { get; set; }
    public decimal? Price { get; set; }
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ImageUrl { get; set; }

    public string? DestinationUrl { get; set; }
    public short? Clicks { get; set; }

    public short? Status { get; set; }

    public string? Type { get; set; }
    public Guid? CreatedBy { get; set; }
    public virtual Admin? CreateAdmin { get; set; }
}
