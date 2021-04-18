using System;
using System.Threading;
using System.Threading.Tasks;
using CoreFX.Abstractions.Contracts.Extensions;
using CoreFX.Caching.Redis.Extensions;
using CoreFX.Common.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using UnitTest.Hello5.Domain.SDK.App_Start;
using Xunit;

namespace UnitTest.Hello5.Domain.SDK.Tests
{
    public class Cache_Test : DerivedUnitTestBase
    {
        [Fact]
        public void CreateCache_Test()
        {
            var cache = GetCache();
            Assert.NotNull(cache);
        }

        [Fact]
        public async Task SetCache_Test()
        {
            var cacheSeconds = 5;
            var cache = GetCache();
            Assert.NotNull(cache);

            var cacheKey = (_cacheKey + Guid.NewGuid().ToString()).ToMD5();
            var cacheVal = DateTime.Now.ToString("s");
            var res = await cache.SetAsync(cacheKey, cacheVal, DateTime.UtcNow.AddSeconds(cacheSeconds));
            Assert.True(res.Any());
            Assert.NotEqual(cacheKey, res.Data);

            RedisCache_Extension.AppendApiNameToCacheKey = false;
            cacheVal = DateTime.Now.ToString("s");
            res = await cache.SetAsync(cacheKey, cacheVal, DateTime.UtcNow.AddSeconds(cacheSeconds));
            RedisCache_Extension.AppendApiNameToCacheKey = true;
            Assert.True(res.Any());
            Assert.Equal(cacheKey, res.Data);
        }

        [Fact]
        public async Task GetCache_Test()
        {
            var cacheSeconds = 5;
            var cache = GetCache();
            Assert.NotNull(cache);

            var cacheKey = (_cacheKey + Guid.NewGuid().ToString()).ToMD5();
            var cacheVal = DateTime.Now.ToString("s");
            var res = await cache.SetAsync(cacheKey, cacheVal, DateTime.UtcNow.AddSeconds(cacheSeconds));
            Assert.True(res.Any());
            Assert.NotEqual(cacheKey, res.Data);

            var cached = await cache.GetAsync<string>(cacheKey);
            Assert.True(cached.Any());
            Assert.Equal(cacheVal, cached.Data);

            Thread.Sleep(cacheSeconds * 1000);

            cached = await cache.GetAsync<string>(cacheKey);
            Assert.False(cached.Any());
        }

        private IDistributedCache GetCache()
        {
            if (_cache == null)
            {
                var services = new ServiceCollection();
                services.AddStackExchangeRedisCache(option =>
                {
                    option.Configuration = "127.0.0.1";
                });

                IServiceProvider provider = services.BuildServiceProvider();
                _cache = provider.GetService<IDistributedCache>();
            }

            return _cache;
        }

        private readonly string _cacheKey = $"UnitTest-{Guid.NewGuid()}";
        private IDistributedCache _cache;
    }
}
