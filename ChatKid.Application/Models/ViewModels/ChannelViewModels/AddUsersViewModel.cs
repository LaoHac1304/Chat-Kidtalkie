namespace ChatKid.Application.Models.ViewModels.ChannelViewModels
{
    public class AddUsersViewModel
    {
        public Guid ChannelId { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>();
    }
}
