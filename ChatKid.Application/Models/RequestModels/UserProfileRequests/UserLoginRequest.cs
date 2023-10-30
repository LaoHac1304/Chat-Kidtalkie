namespace ChatKid.Application.Models.RequestModels.UserProfileRequests
{
    public class UserLoginRequest
    {
        public Guid Id {  get; set; }
        public string? Password { get; set; }
    }
}
