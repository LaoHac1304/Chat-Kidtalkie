namespace ChatKid.Application.Models.RequestModels.FamilyRequests
{
    public class FamilyUpdateRequest
    {
        public string? Name { get; set; }

        public string? AvatarUrl { get; set; }

        public short? Status { get; set; } = 1;
    }
}
