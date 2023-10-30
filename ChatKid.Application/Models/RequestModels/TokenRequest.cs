namespace ChatKid.Application.Models.RequestModels
{
    public record TokenRequest
    ( 
        string AccessToken,
        string RefreshToken
    );
}
