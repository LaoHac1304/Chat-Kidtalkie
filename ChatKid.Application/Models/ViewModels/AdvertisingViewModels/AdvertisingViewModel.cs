namespace ChatKid.Application.Models.ViewModels.AdvertisingViewModels
{
    public class AdvertisingViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Company { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ImageUrl { get; set; }

        public string? DestinationUrl { get; set; }
        public string? Type { get; set; }

        public short? Clicks { get; set; }
        public short? Status { get; set; }
    }
}
