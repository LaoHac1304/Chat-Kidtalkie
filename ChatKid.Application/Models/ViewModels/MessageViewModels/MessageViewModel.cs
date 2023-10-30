namespace ChatKid.Application.Models.ViewModels.MessageViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public string? VoiceUrl { get; set; }

        public DateTime? SentTime { get; set; }

        public Guid ChannelUserId { get; set; }
        public short? Status { get; set; }
    }
}
