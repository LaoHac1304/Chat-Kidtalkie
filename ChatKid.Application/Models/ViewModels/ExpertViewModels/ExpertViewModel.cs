using ChatKid.Application.Models.ViewModels.DiscussRoomViewModels;
using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.ViewModels.ExpertViewModels
{
    public class ExpertViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gmail { get; set; }

        public string Phone { get; set; }

        public int? Age { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public virtual ICollection<DiscussRoomViewModel> DiscussRooms { get; set; }

        public short? Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
