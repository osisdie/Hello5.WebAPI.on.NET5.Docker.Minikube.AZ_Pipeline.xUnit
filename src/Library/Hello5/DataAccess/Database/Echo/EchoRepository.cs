using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using CoreFX.Abstractions.Bases;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Contracts.Extensions;
using CoreFX.Abstractions.Contracts.Interfaces;
using CoreFX.Abstractions.Enums;
using CoreFX.Caching.Redis.Extensions;
using Hello5.Domain.Common;
using Hello5.Domain.DataAccess.Database.Echo.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Hello5.Domain.DataAccess.Database
{
    public class EchoRepository : FxObject, IEchoRepository
    {
        private IDistributedCache _distributedCache;
        private static readonly string _connectionString;

        static EchoRepository()
        {
            _connectionString = HelloContext.Settings.HELLODB_CONN;
        }

        public EchoRepository(IDistributedCache distributedCache = null)
        {
            _distributedCache = distributedCache;
        }

        public EchoRepository()
        {

        }

        public async Task<ISvcResponse> SetVerision(string version)
        {
            var res = new SvcResponse<string>(true);
            if (_distributedCache == null)
            {
                return res;
            }

            var cacheKey = $"{GetType()}{SvcConst.Separator}{MethodBase.GetCurrentMethod().Name}";
            var cacheResult = await _distributedCache.SetAsync<string>(cacheKey, version, DateTime.UtcNow.AddMinutes(1));
            if (cacheResult.Any())
            {
                res.Success();
                return res;
            }
            else
            {
                _distributedCache = null;
            }

            return res;
        }

        public async Task<ISvcResponse<string>> GetVerision()
        {
            if (_distributedCache == null)
            {
                return await InnerGetVerision();
            }

            var res = new SvcResponse<string>();
            var cacheKey = $"{GetType()}{SvcConst.Separator}{MethodBase.GetCurrentMethod().Name}";
            var cacheResult = await _distributedCache.GetAsync<string>(cacheKey);
            if (cacheResult.Any())
            {
                res.Success().SetData(cacheResult.Data);
                return res;
            }

            return await InnerGetVerision();
        }

        private async Task<ISvcResponse<string>> InnerGetVerision()
        {
            var res = new SvcResponse<string>();
            try
            {
                var connString = _connectionString;
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (var sqlcmd = new SqlCommand("SELECT @@VERSION", conn))
                    {
                        var dbResult = await sqlcmd.ExecuteScalarAsync();
                        res.Success().SetData(dbResult?.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
                res.Error(SvcCodeEnum.Error, ex.Message);
            }

            return res;
        }
    }
}
