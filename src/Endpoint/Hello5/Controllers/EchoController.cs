using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Contracts.Extensions;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Logging.Extensions;
using CoreFX.Abstractions.Utils;
using CoreFX.Caching.Redis.Extensions;
using CoreFX.Common.Extensions;
using Hello5.Domain.Common;
using Hello5.Domain.Contract.Models.Echo;
using Hello5.Domain.DataAccess.Database.Echo.Interfaces;
using Hello5.Domain.Endpoint.Controllers.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Hello5.Domain.Endpoint.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class EchoController : HelloContollerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IEchoRepository _repository;

        public EchoController(IEchoRepository repository, IDistributedCache distributedCache = null)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("api/echo/ver")]
        [Route("api/echo/version")]
        public async Task<ActionResult> EchoVersion()
        {
            var res = new HelloEchoVersionResponseDto
            {
                Data = SdkRuntime.Version
            }.Success();

            await Task.CompletedTask;
            return new JsonResult(res);
        }

        [HttpGet]
        [Route("api/echo/db")]
        public async Task<ActionResult> EchoDB(string ver = null)
        {
            var res = new HelloEchoDBResponseDto();
            if (!string.IsNullOrEmpty(ver))
            {
                await _repository.SetVerision(ver);
            }

            var dbResult = await _repository.GetVerision();
            if (dbResult.Any())
            {
                res.Success(SvcCodeEnum.Success, dbResult.Msg).SetData(dbResult.Data);
            }
            else
            {
                res.Error(SvcCodeEnum.Error, dbResult?.Msg ?? SvcMsg.DatabaseUnavailable);
            }

            return new JsonResult(res)
            {
                StatusCode = (int)(res.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable)
            };
        }

        [HttpGet]
        [Route("api/echo/cache")]
        public async Task<ActionResult> EchoCache()
        {
            var res = new HelloEchoCacheResponseDto();
            var cacheKey = $"{GetType()}{SvcConst.Separator}{NetworkUtil.LocalIP}";

            if (_distributedCache != null)
            {
                var cacheResult = await _distributedCache.SetAsync(cacheKey, SvcConst.DefaultHealthyResponse, DateTime.UtcNow.AddSeconds(5));
                if (cacheResult.Any())
                {
                    res.Success(SvcCodeEnum.Success, cacheResult.Msg).SetData(cacheResult.Data);
                }
                else
                {
                    res.Error(SvcCodeEnum.Error, cacheResult?.Msg ?? SvcMsg.CacheUnavailable);
                }
            }
            else
            {
                res.Error(SvcCodeEnum.Error, SvcMsg.CacheUnavailable);
            }

            return new JsonResult(res)
            {
                StatusCode = (int)(res.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable)
            };
        }

        [HttpGet]
        [Route("api/echo/config")]
        public async Task<ActionResult> EchoConfig()
        {
            var appSettingConfig = System.IO.File.Exists(
                SvcConst.DefaultAppSettingsFile.AddingBeforeExtension(SdkRuntime.SdkEnv));
            var helloSettingConfig = System.IO.File.Exists(
                HelloContext.Settings.Name);
            var logConfig = System.IO.File.Exists(
                Path.Combine(SvcConst.DefaultConfigFolder, SvcConst.DefaultLog4netConfigFile.AddingBeforeExtension(SdkRuntime.SdkEnv)));

            var res = new HelloEchoConfigResponseDto();
            res.SetStatusCode((appSettingConfig & helloSettingConfig & logConfig))
                .SetData(new Dictionary<string, bool>
                {
                    {nameof(appSettingConfig), appSettingConfig },
                    {nameof(helloSettingConfig), helloSettingConfig },
                    {nameof(logConfig), logConfig },
                });

            await Task.CompletedTask;
            return new JsonResult(res)
            {
                StatusCode = (int)(res.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable)
            };
        }

        [HttpGet]
        [Route("api/echo/dump")]
        public async Task<ActionResult> EchoDump()
        {
            var res = new HelloEchoDumpResponseDto();
            res.Success();
            res.ExtMap.AddDebugData();

            await Task.CompletedTask;
            return new JsonResult(res);
        }
    }
}
