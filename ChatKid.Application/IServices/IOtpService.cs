using ChatKid.Application.Models.ViewModels.OtpViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IOtpService
    {
        Task<List<OtpViewModel>> GetOtpsAsync();
        Task<bool> CheckOtp(string email, int otp);
        Task<CommandResult> UpdateOtp(string email, int otp);
        Task<CommandResult> CreateOtp(string email, int otp);
    }
}
