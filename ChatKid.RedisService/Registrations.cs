using ChatKid.DataLayer;
using ChatKid.RedisService.OtpCaching;
using ChatKid.RedisService.RedisCaching;
using ChatKid.RedisService.RefreshTokenCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ChatKid.RedisService
{
    public static class Registrations
    {
        public static IServiceCollection RegisterRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDatabase>(provider =>
            {
                var connectionString = configuration.GetSection("ConnectionStrings");
                var redis = ConnectionMultiplexer.Connect(connectionString["RedisDb"]);
                return redis.GetDatabase();
            });
            services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));

            services.AddScoped<IRefreshTokenCachingService, RefreshTokenCachingService>();
            services.AddScoped<IOtpCachingService, OtpCachingService>();
            return services;
        }

    }
}