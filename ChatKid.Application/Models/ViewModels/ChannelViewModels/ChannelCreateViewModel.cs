using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.ViewModels.ChannelViewModels
{
    public class ChannelCreateViewModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>();

        public virtual ICollection<ChannelUser> ChannelUsers { get; set; }
            = new List<ChannelUser>();

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public short? Status { get; set; }

    }
}
