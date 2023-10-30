namespace ChatKid.Application.Models.RequestModels.ChannelRequests
{
    public class AddUsersRequest
    {
        public Guid ChannelId { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>();
    }
}
