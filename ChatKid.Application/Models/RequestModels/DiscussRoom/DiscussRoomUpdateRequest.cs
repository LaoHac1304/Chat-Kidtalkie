using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.Models.RequestModels.DiscussRoom
{
    public class DiscussRoomUpdateRequest
    {
        public Guid? KidServiceId { get; set; }
        public Guid? ExpertId { get; set; }
        public string? VoiceUrl { get; set; }
    }
}
