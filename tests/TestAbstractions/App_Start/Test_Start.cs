using System.IO;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Logging;
using CoreFX.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestAbstractions.Extensions;

namespace TestAbstractions.App_Start
{
    public static class Test_Start
    {
        public static void Configure(this UnitTestBase app)
        {
            LaunchSettingsExtension.SetEnvironmentVariables();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            var configPathList = app.GetConfigPathList();
            foreach (var configPath in configPathList)
            {
                builder.AddJsonFile(configPath, optional: false);
            }

            SdkRuntime.Configuration = builder.Build();

            var logPath = app.GetLogPath();
            if (File.Exists(logPath))
            {
                var loggerFactory = new LoggerFactory();
                loggerFactory.AddLog4Net(SvcConst.DefaultLog4netConfigFile);
                LogMgr.LoggerFactory = loggerFactory;
            }

            SvcContext.InitialSDK();
            app.AfterConfigured(SdkRuntime.Configuration, LogMgr.LoggerFactory);
        }
    }
}
