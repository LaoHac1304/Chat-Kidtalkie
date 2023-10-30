namespace ChatKid.Application.Models.RequestModels.MessageRequests
{
    public class MessageUpdateRequest
    {
        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public string? VoiceUrl { get; set; }
        public short? Status { get; set; } = 1;
    }
}
