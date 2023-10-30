using ChatKid.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChatKid.Application.Models.RequestModels.ChannelRequests
{
    public class ChannelCreateRequest
    {
        public string Name { get; set; }
        public List<Guid> UserIds { get; set; } 
    }
}
