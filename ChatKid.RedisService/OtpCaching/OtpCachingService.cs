using ChatKid.RedisService.Models;
using ChatKid.RedisService.RedisCaching;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Linq.Dynamic.Core.Tokenizer;

namespace ChatKid.RedisService.OtpCaching
{
    public class OtpCachingService : CacheService<Dictionary<string, OtpCachedModel>>, IOtpCachingService
    {
        public OtpCachingService(IDatabase db) : base(db)
        {
        }

        public async Task<bool> IsValidOtpAsync(string email, int otp)
        {
            Dictionary<string, OtpCachedModel> otps = await base.GetAsync<Dictionary<string, OtpCachedModel>>(KeyConstants.OtpKey);
            if (otps is null || !otps.ContainsKey(email) || otps[email].Expired < DateTime.Now) return false;
            return otps[email].Otp == otp;
        }

        public async Task<bool> SaveAsync(string email, int otp)
        {
            OtpCachedModel model = new()
            {
                Otp = otp
            };
            Dictionary<string, OtpCachedModel> otps = await base.GetAsync<Dictionary<string, OtpCachedModel>>(KeyConstants.OtpKey);
            if (otps is null) otps = new Dictionary<string, OtpCachedModel>();
            if (otps.ContainsKey(email)) otps[email] = model;
            else otps.Add(email, model);

            return await base.SetAsync(KeyConstants.OtpKey, otps);
        }

    }
}
