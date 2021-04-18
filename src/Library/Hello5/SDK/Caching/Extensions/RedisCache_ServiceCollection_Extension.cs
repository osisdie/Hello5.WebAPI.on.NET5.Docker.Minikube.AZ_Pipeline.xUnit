using System;
using CoreFX.Caching.Redis.Consts;
using Hello5.Domain.Common.Consts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Hello5.Domain.SDK.Caching.Extensions
{
    public static class RedisCache_ServiceCollection_Extension
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var connectionString = configuration?.GetValue<string>(HelloConst.DefaultCacheConnectionKey);
                if (!string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration?.GetValue<string>(CacheConst.DefaultConnectionKey);
                }

                if (!string.IsNullOrEmpty(connectionString))
                {
                    var options = ConfigurationOptions.Parse(connectionString);
                    services.AddStackExchangeRedisCache(option =>
                    {
                        option.ConfigurationOptions = options;
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return services;
        }
    }
}
