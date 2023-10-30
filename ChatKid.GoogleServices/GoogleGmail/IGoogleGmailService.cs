
using ChatKid.Common.CommandResult;
using Google.Apis.Gmail.v1.Data;

namespace ChatKid.GoogleServices.GoogleGmail
{
    public interface IGoogleGmailService
    {
        Task<CommandResult> SendOTP(string email);
        Task<bool> VerifyOTP(string email, int otp);
    }
}
