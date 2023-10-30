namespace ChatKid.RedisService.RedisCaching
{
    public interface ICacheService<T>
    {
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration);
        Task<bool> RemoveAsync(string key);
        Task<bool> ExistAsync(string key);
        Task ClearAsync();  
    }
}
