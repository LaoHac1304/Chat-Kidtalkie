using ChatKid.Application.Models.ViewModels.AdminViewModels;

namespace ChatKid.Application.Models.ViewModels.NotificationViewModels
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        
        public string? Content { get; set; } 
        public string? Receiver { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
        public string? CreatorEmail { get; set; }
        public short? Status { get; set; }
    }
}
