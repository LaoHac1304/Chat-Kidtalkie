using ChatKid.RedisService.Models;
using ChatKid.RedisService.RedisCaching;
using StackExchange.Redis;

namespace ChatKid.RedisService.RefreshTokenCaching
{
    public class RefreshTokenCachingService : CacheService<Dictionary<string, RefreshTokenCachedModel>>, IRefreshTokenCachingService
    {
        public RefreshTokenCachingService(IDatabase db) : base(db)
        {
        }

        public async Task<bool> IsValidTokenAsync(string email, string token)
        {
            Dictionary<string, RefreshTokenCachedModel> refreshTokens = await base.GetAsync<Dictionary<string, RefreshTokenCachedModel>>(KeyConstants.RefreshTokenKey);
            if (refreshTokens is null || !refreshTokens.ContainsKey(email) || refreshTokens[email].Expired < DateTime.Now) return false;
            return refreshTokens[email].RefreshToken == token;
        }

        public async Task<bool> SaveSync(string email, string token)
        {
            RefreshTokenCachedModel model = new RefreshTokenCachedModel()
            {
                RefreshToken = token
            };
            Dictionary<string, RefreshTokenCachedModel> refreshTokens = await base.GetAsync<Dictionary<string, RefreshTokenCachedModel>>(KeyConstants.RefreshTokenKey);
            if (refreshTokens is null) refreshTokens = new Dictionary<string, RefreshTokenCachedModel>();
            if(refreshTokens.ContainsKey(email)) refreshTokens[email] = model;
            else refreshTokens.Add(email, model);

            return await base.SetAsync(KeyConstants.RefreshTokenKey, refreshTokens);
        }
    }
}
