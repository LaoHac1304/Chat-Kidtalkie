using ChatKid.Common.CommandResult;

namespace ChatKid.GoogleServices.GoogleAuthentication
{
    public interface IGoogleAuthenticationService
    {
        Task<CommandResult> GoogleLogin(string token);
    }
}
