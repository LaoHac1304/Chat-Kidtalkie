namespace ChatKid.Application.Models.ViewModels.UserViewModels
{
    public class UserFamilyViewModel
    {
        public Guid Id { get; set; }

        public string AvatarUrl { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public short? Status { get; set; }

        public Guid? FamilyId { get; set; }

        public string DeviceToken { get; set; }

        public string FamilyName { get; set; }

        public string OwnerMail { get; set; }

        public string? Gender { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public short? IsUpdated { get; set; }

    }
}
