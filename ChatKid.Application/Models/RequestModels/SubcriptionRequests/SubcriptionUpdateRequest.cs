namespace ChatKid.Application.Models.RequestModels.SubcriptionRequests
{
    public class SubcriptionUpdateRequest
    {
        public decimal? Price { get; set; }

        public decimal? ActualPrice { get; set; }

        public string? Name { get; set; }

        public short? Energy { get; set; }

        public short? Status { get; set; } = 1;
    }
}
