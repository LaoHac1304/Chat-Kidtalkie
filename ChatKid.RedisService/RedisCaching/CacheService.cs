using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChatKid.RedisService.RedisCaching
{
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IDatabase _db;
        public CacheService(IDatabase db) {
            _db = db;
        }

        public async Task ClearAsync()
        {
            await _db.ExecuteAsync("FLUSHDB");
        }

        public async Task<bool> ExistAsync(string key)
        {
            var isExist = await _db.KeyExistsAsync(key);
            return isExist;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }   
            return default;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            if (ExistAsync(key).Result)
            {
                return await _db.KeyDeleteAsync(key);
            }
            return false;
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            var data = JsonConvert.SerializeObject(value);
            return await _db.StringSetAsync(key, data);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var data = JsonConvert.SerializeObject(value);
            return await _db.StringSetAsync(key, data, expiration);
        }
    }
}
