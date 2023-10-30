using ChatKid.Application.Models.ViewModels.UserViewModels;
using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.ViewModels.FamilyViewModels
{
    public class FamilyViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OwnerMail { get; set; }

        public string AvatarUrl { get; set; }

        public short? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserViewModel> Users { get; set; }

        public ICollection<ParentSubcription> ParentSubcriptions { get; set; }
    }

}
