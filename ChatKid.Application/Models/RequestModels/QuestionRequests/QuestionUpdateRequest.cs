namespace ChatKid.Application.Models.RequestModels.QuestionRequests
{
    public class QuestionUpdateRequest
    { 
        public string? Content { get; set; }

        public string? VoiceUrl { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string? Answer { get; set; }
        public string? Form { get; set; }

        public Guid? KidServiceId { get; set; }
        public short? Status { get; set; }
    }
}
