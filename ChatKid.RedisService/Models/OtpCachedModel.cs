using ChatKid.Common.Constants;

namespace ChatKid.RedisService.Models
{
    public class OtpCachedModel
    {
        public int Otp { get; set; } = 0;
        public DateTime Expired = DateTime.Now.AddSeconds(ExpiredConstants.OtpExpiredSecond);
    }
}
