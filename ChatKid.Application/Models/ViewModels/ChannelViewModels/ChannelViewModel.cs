using ChatKid.Application.Models.ViewModels.ChannelUserViewModels;

namespace ChatKid.Application.Models.ViewModels.ChannelViewModels
{
    public class ChannelViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public byte? Status { get; set; }

        public ICollection<ChannelUserViewModel> ChannelUsers { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}
