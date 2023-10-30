using ChatKid.Application.IServices;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Constants;
using ChatKid.Common.Logger;
using ChatKid.DataLayer;
using ChatKid.DataLayer.Entities;
using ChatKid.GoogleServices.GoogleAuthentication;
using ChatKid.GoogleServices.GoogleSettings;
using ChatKid.RedisService.OtpCaching;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;

namespace ChatKid.GoogleServices.GoogleGmail
{
    public class GoogleGmailService : IGoogleGmailService
    {
        private readonly AppGmailSettings appGmailSettings;
        private readonly IOtpCachingService _otpCachingService;
        public GoogleGmailService(AppGmailSettings appGmailSettings, IOtpCachingService otpCachingService)
        {
            this.appGmailSettings = appGmailSettings;
            this._otpCachingService = otpCachingService;
        }
        public async Task<CommandResult> SendOTP(string email)
        {
            var fromAddress = new MailAddress(appGmailSettings.Email);
            var toAddress = new MailAddress(email);
            string subject = "Kidtalkie OTP Verification";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, appGmailSettings.AppPassword)
            };
            int otp = this.GenerateOTP();
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = "Please use this verification code to access to KidTalkie <br/>" +
                "<strong style=\\\"font-size:25px;\\\">" + otp.ToString() + "</strong>"
            })
            {
                try
                {
                    smtp.Send(message);
                }catch (Exception ex)
                {
                    Logger<GoogleGmailService>.Error(ex, "Data: " + email);
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Description = "SendOTP " + ex
                    });
                }
                
            }
            var result = await _otpCachingService.SaveAsync(email, otp);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = String.Format(CommandMessages.RedisSavedFailed, email + "-" + otp)
            });
            return CommandResult.SuccessWithData(otp);
        }

        public async Task<bool> VerifyOTP(string email, int otp) => await _otpCachingService.IsValidOtpAsync(email, otp);

        private int GenerateOTP() => new Random().Next(100000, 999999);
    }
}
