using ChatKid.RedisService.Models;
using ChatKid.RedisService.RedisCaching;

namespace ChatKid.RedisService.RefreshTokenCaching
{
    public interface IRefreshTokenCachingService : ICacheService<Dictionary<string, RefreshTokenCachedModel>>
    {
        Task<bool> IsValidTokenAsync(string email, string token);
        Task<bool> SaveSync(string email, string token);
    }
}
