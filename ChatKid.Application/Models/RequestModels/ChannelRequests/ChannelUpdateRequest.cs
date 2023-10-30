namespace ChatKid.Application.Models.RequestModels.ChannelRequests
{
    public class ChannelUpdateRequest
    {
        public string? Name { get; set; }
        public byte? Status { get; set; } = 1;
    }
}
