using ChatKid.DataLayer.EntityInterfaces;

namespace ChatKid.DataLayer.Entities;

public partial class Subcription : IBaseEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public decimal? Price { get; set; }

    public decimal? ActualPrice { get; set; }

    public string? Name { get; set; }

    public short? Energy { get; set; }

    public short? Status { get; set; }

    public virtual ICollection<ParentSubcription> ParentSubcriptions { get; set; } = new List<ParentSubcription>();
}
