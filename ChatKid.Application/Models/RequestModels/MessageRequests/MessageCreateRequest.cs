using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.RequestModels.MessageRequests
{
    public class MessageCreateRequest
    {
        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public string? VoiceUrl { get; set; }

        public Guid ChannelUserId { get; set; }
        public short? Status { get; set; } = 1;
    }
}
