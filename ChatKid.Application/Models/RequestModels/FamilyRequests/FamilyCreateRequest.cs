using Microsoft.AspNetCore.Mvc;

namespace ChatKid.Application.Models.RequestModels.FamilyRequests
{
    public class FamilyCreateRequest
    {
        public string? Name { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
