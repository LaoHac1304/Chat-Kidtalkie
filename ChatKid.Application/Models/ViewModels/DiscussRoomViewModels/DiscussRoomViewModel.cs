using ChatKid.Application.Models.ViewModels.ExpertViewModels;
using ChatKid.Application.Models.ViewModels.UserViewModels;
using ChatKid.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.Models.ViewModels.DiscussRoomViewModels
{
    public class DiscussRoomViewModel
    {
        public Guid Id { get; set; }

        public string? VoiceUrl { get; set; }
        public DateTime? CreatedTime { get; set; }

        public Guid ExpertId { get; set; }
        public virtual UserViewModel Children { get; set; }
    }
}
