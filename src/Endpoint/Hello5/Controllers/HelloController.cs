using System;
using System.Threading.Tasks;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Contracts.Extensions;
using Hello5.Domain.Contract.Models.Echo;
using Hello5.Domain.Contract.Models.Extensions;
using Hello5.Domain.Endpoint.Controllers.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Hello5.Domain.Endpoint.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class HelloController : HelloContollerBase
    {
        [HttpPost]
        [Route("api/hello/echo")]
        public async Task<ActionResult> Echo(HelloEchoRequestDto requestDto)
        {
            return await EchoV2(requestDto);
        }

        [ApiVersion("1.0")]
        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost]
        [Route("api/v1/hello/echo")]
        public async Task<ActionResult> EchoV1(HelloEchoRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            requestDto.PreProcess();

            var fakeSessionId = DateTime.UtcNow.Ticks.ToString();
            await Task.CompletedTask;
            return new OkObjectResult(fakeSessionId);
        }

        [ApiVersion("2.0")]
        [ApiExplorerSettings(GroupName = "v2")]
        [HttpPost]
        [Route("api/v2/hello/echo")]
        public async Task<ActionResult> EchoV2(HelloEchoRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            requestDto.PreProcess();
            var res = new SvcResponse<HelloEchoResponseDto>();
            var fakeSessionId = DateTime.UtcNow.Ticks.ToString();

            res.Data.Recv = fakeSessionId;
            res.Success();

            await Task.CompletedTask;
            return new JsonResult(res);
        }
    }
}
