using ChatKid.RedisService.Models;
using ChatKid.RedisService.RedisCaching;

namespace ChatKid.RedisService.OtpCaching
{
    public interface IOtpCachingService : ICacheService<Dictionary<string, OtpCachedModel>>
    {
        Task<bool> IsValidOtpAsync(string email, int otp);
        Task<bool> SaveAsync(string email, int otp);
    }
}
