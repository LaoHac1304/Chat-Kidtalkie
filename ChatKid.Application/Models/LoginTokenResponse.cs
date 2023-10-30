namespace ChatKid.Application.Models
{
    public class LoginTokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
    }
}
