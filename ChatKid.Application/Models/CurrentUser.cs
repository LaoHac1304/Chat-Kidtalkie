namespace ChatKid.Application.Models
{
    public class CurrentUser
    {
        public string? AvatarUrl { get; set; }

        public string? Name { get; set; }

        public string? Role { get; set; }

        public short? Status { get; set; }

        public Guid? FamilyId { get; set; }

        public string CurrentUserId { get; set; }

        public string? Gender;
    }
}
