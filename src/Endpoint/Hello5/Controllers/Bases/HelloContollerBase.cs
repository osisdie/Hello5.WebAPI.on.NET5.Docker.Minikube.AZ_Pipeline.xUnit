using CoreFX.Abstractions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hello5.Domain.Endpoint.Controllers.Bases
{
    public abstract class HelloContollerBase : ControllerBase
    {
        protected readonly ILogger _logger;

        protected HelloContollerBase()
        {
            _logger = LogMgr.CreateLogger(GetType());
        }
    }
}
