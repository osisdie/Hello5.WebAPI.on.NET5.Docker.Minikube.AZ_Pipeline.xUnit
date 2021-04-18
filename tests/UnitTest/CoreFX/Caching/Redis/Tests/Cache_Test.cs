using System;
using System.Threading;
using System.Threading.Tasks;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Contracts.Extensions;
using CoreFX.Abstractions.Utils;
using CoreFX.Caching.Redis.Extensions;
using CoreFX.Common.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using UnitTest.CoreFX.Caching.Redis.App_Start;
using Xunit;

namespace UnitTest.CoreFX.Caching.Redis.Tests
{
    public class Cache_Test : DerivedUnitTestBase
    {
        [Fact]
        public async Task Integration_Test()
        {
            CreateCache_Test();
            await SetCache_Test();
            await GetCache_Test();
            await UnhealthyCache_Test();
            await ReconnectCache();
            await GetCache_Test();
        }

        [Fact(Skip = "Use Integration_Test")]
        public void CreateCache_Test()
        {
            var cache = GetCache();
            Assert.NotNull(cache);
        }

        [Fact(Skip = "Use Integration_Test")]
        public async Task SetCache_Test()
        {
            var cacheSeconds = 5;
            var cache = GetCache();
            Assert.NotNull(cache);

            var cacheKey = (_cacheKey + Guid.NewGuid().ToString()).ToMD5();
            var cacheVal = DateTime.Now.ToString("s");
            var res = await cache.SetAsync(cacheKey, cacheVal, DateTime.UtcNow.AddSeconds(cacheSeconds));
            Assert.True(res.Any(), res?.Msg);
            Assert.NotEqual(cacheKey, res.Data);

            cacheKey = $"{cacheKey}{SvcConst.Separator}{Guid.NewGuid()}";
            RedisCache_Extension.AppendApiNameToCacheKey = false;
            cacheVal = DateTime.Now.ToString("s");
            res = await cache.SetAsync(cacheKey, cacheVal, DateTime.UtcNow.AddSeconds(cacheSeconds));
            RedisCache_Extension.AppendApiNameToCacheKey = true;
            Assert.True(res.Any());
            Assert.Equal(cacheKey, res.Data);
        }

        [Fact(Skip = "Use Integration_Test")]
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

        [Fact(Skip = "Use Integration_Test")]
        public async Task UnhealthyCache_Test()
        {
            var services = new ServiceCollection();
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "123"; // invalid connection string
            });

            IServiceProvider provider = services.BuildServiceProvider();
            var cache = provider.GetService<IDistributedCache>();
            Assert.NotNull(cache);

            var failbackScore = FailbackScoreControl.CreateNew;
            var cacheKey = (_cacheKey + Guid.NewGuid().ToString()).ToMD5();
            var cached = await cache.GetAsync<string>(cacheKey);
            Assert.False(cached.IsSuccess);

            for (var i = 0; i < 5; ++i)
            {
                if (cache.IsUnavailable())
                {
                    cached = await cache.GetAsync<string>(cacheKey);
                    Assert.False(cached.IsSuccess);
                    Assert.Equal(cached.Msg, SvcMsg.CacheUnavailable);
                    break;
                }
                cached = await cache.GetAsync<string>(cacheKey);
                Assert.False(cached.IsSuccess);
            }
        }

        private async Task ReconnectCache()
        {
            if (_cache != null && _cache.IsUnavailable())
            {
                var retryResult = await _cache.TryReconnect();
                Assert.True(retryResult?.IsSuccess);
                Assert.False(_cache.IsUnavailable());
            }
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
