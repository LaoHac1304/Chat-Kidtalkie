namespace ChatKid.Application.Models.RequestModels.Admin
{
    public class AdminUpdateRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public short? Status { get; set; }
        public string? Avatar { get; set; } 
    }
}
