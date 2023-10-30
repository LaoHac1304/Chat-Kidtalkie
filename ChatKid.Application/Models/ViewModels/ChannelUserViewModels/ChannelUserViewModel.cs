namespace ChatKid.Application.Models.ViewModels.ChannelUserViewModels
{
    public class ChannelUserViewModel
    {
        public Guid Id { get; set; }

        public Guid ChannelId { get; set; }

        public Guid UserId { get; set; }

        public string? NameInChannel { get; set; }

        public short? Status { get; set; }
    }
}
