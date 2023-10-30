namespace ChatKid.Application.Models.RequestModels.BlogRequests
{
    public class BlogUpdateRequest
    {
        public string? Title { get; set; }

        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VoiceUrl { get; set; }
        public short? Status { get; set; }
        public Guid? TypeBlogId { get; set; }
    }
}
