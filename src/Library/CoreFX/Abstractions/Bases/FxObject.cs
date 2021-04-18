using System;
using CoreFX.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace CoreFX.Abstractions.Bases
{
    public abstract class FxObject
    {
        protected FxObject()
        {
            if (_logger == null)
            {
                _logger = LogMgr.CreateLogger(GetType());
            }
        }

        public ILogger GetLogger() => _logger;
        public readonly Guid _id = Guid.NewGuid();
        public readonly DateTime _ts = DateTime.UtcNow;

        protected ILogger _logger;
    }
}
