using System;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CoreFX.Caching.Redis.Extensions
{
    public static class RedisCache_ServiceCollection_Extension
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
        {
            try
            {
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
