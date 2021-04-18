using System;
using System.Collections.Generic;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Consts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestAbstractions.App_Start
{
    public abstract class UnitTestBase
    {
        protected UnitTestBase()
        {
            Initialization();
        }

        public virtual List<string> GetConfigPathList() => new List<string> { SvcConst.DefaultAppSettingsFile };
        public virtual string GetLogPath() => SvcConst.DefaultLog4netConfigFile;
        public virtual void AfterConfigured(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Console.WriteLine($"{EnvConst.AspNetCoreEnvironment}={SdkRuntime.SdkEnv}");
        }

        private void Initialization()
        {
            if (_isInitialized)
            {
                return;
            }

            lock (_lock)
            {
                if (!_isInitialized)
                {
                    this.Configure();
                }
            }
        }

        protected static bool _isInitialized = false;
        private static readonly object _lock = new object();
    }
}
