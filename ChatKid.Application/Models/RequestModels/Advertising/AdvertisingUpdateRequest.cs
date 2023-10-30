namespace ChatKid.Application.Models.RequestModels.Advertising
{
    public class AdvertisingUpdateRequest
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Company { get; set; }
        public string? CompanyEmail { get; set; }
        public decimal? Price { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ImageUrl { get; set; }

        public string? DestinationUrl { get; set; }
        public short? Status { get; set; }
        public string? Type { get; set; }

    }
}
