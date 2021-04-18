using System;
using System.Threading.Tasks;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Contracts.Extensions;
using CoreFX.Abstractions.Contracts.Interfaces;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Logging;
using CoreFX.Abstractions.Serializers;
using CoreFX.Abstractions.Utils;
using CoreFX.Common.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CoreFX.Caching.Redis.Extensions
{
    public static class RedisCache_Extension
    {
        public static readonly IFailbackScore Status;
        private static readonly ILogger _logger;
        static RedisCache_Extension()
        {
            _logger = LogMgr.CreateLogger(typeof(RedisCache_Extension));
            Status = FailbackScoreControl.CreateNew;
        }

        public static async Task<ISvcResponse<T>> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var res = new SvcResponse<T>();
            if (Status.IsExceedLimit())
            {
                return res.Error(SvcCodeEnum.Error, SvcMsg.CacheUnavailable);
            }

            var cacheKey = AppendApiNameToCacheKey ? $"{SdkRuntime.ApiName}-{key}" : key;
            try
            {
                var cacheResult = await cache.GetStringAsync(cacheKey);
                if (cacheResult != null)
                {
                    res.SetData(typeof(T) == typeof(string)
                        ? (T)Convert.ChangeType(cacheResult, typeof(string))
                        : DefaultJsonSerializer.Deserialize<T>(cacheResult)
                    );
                }

                Status.Score(true);
                res.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                Status.Fail();
                res.Error(SvcCodeEnum.Error, ex.Message);
            }

            res.ExtMap["CacheKey"] = cacheKey;
            return res;
        }

        public static async Task<ISvcResponse<string>> SetAsync<T>(this IDistributedCache cache, string key, T value, DateTimeOffset expireTime)
        {
            var res = new SvcResponse<string>();
            var cacheKey = AppendApiNameToCacheKey ? $"{SdkRuntime.ApiName}-{key}" : key;
            res.SetData(cacheKey);

            if (Status.IsExceedLimit())
            {
                return res.Error(SvcCodeEnum.Error, SvcMsg.CacheUnavailable);
            }

            try
            {
                var stringValue = typeof(T) == typeof(string)
                    ? (string)Convert.ChangeType(value, typeof(string))
                    : DefaultJsonSerializer.Serialize(value);
                await cache.SetStringAsync(cacheKey, stringValue, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = expireTime
                });

                Status.Score(true);
                res.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                Status.Fail();
                res.Error(SvcCodeEnum.Error, ex.Message);
            }

            return res;
        }

        public static async Task<ISvcResponse> TryReconnect(this IDistributedCache cache)
        {
            var res = new SvcResponse();
            try
            {
                var cacheKey = ($"{SdkRuntime.ApiName}{SvcConst.Separator}{Guid.NewGuid()}").ToMD5();
                await cache.SetStringAsync(cacheKey, DateTime.UtcNow.ToString("s"), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddSeconds(5)
                });
                Status.Reset();
                res.Success();
            }
            catch (Exception ex)
            {
                res.Error(SvcCodeEnum.Error, ex.Message);
            }

            return res;
        }

        public static bool IsUnavailable(this IDistributedCache cache) => Status.IsExceedLimit();

        public static bool AppendApiNameToCacheKey = true;
    }
}
