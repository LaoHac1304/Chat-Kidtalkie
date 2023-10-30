using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.ViewModels.SubcriptionViewModels
{
    public class SubcriptionViewModel
    {
        public Guid Id { get; set; }

        public decimal? Price { get; set; }

        public decimal? ActualPrice { get; set; }

        public string? Name { get; set; }

        public short? Energy { get; set; }

        public short? Status { get; set; }

    }
}
