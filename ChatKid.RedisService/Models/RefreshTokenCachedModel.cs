using ChatKid.Common.Constants;

namespace ChatKid.RedisService.Models
{
    public class RefreshTokenCachedModel
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expired = DateTime.Now.AddSeconds(ExpiredConstants.RefreshTokenExpiredSecond);
    }
}
