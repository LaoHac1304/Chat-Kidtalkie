namespace ChatKid.Application.Models.ViewModels
{
    public class FilterViewModel
    {
        public string? SearchString { get; set; } = "";

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public short? Status { get; set; } = 2;

        public string? Email { get; set; } = "";
    }
}
