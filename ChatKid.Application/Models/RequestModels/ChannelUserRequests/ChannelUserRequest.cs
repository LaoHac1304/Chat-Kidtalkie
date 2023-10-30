using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.RequestModels.ChannelUserRequests
{
    public class ChannelUserRequest
    {
        public Guid ChannelId { get; set; }

        public Guid UserId { get; set; }

        public string? NameInChannel { get; set; }
        public short? Status { get; set; } = 1;

    }
}
