using System.IO;
using System.Threading.Tasks;
using CoreFX.Abstractions.Consts;
using CoreFX.Common;
using CoreFX.Common.Extensions;
using Hello5.Domain.Common.Consts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hello5.Domain.Endpoint
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            SvcContext.InitialSDK();
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(SvcConst.DefaultAppSettingsFile, optional: false, reloadOnChange: true)
                        .AddJsonFile(SvcConst.DefaultAppSettingsFile.AddingBeforeExtension(env.EnvironmentName), optional: true, reloadOnChange: true)
                        .AddJsonFile(Path.Combine(SvcConst.DefaultConfigFolder, HelloConst.DefaultConfigFile), optional: false, reloadOnChange: true)
                        .AddJsonFile(Path.Combine(SvcConst.DefaultConfigFolder, HelloConst.DefaultConfigFile.AddingBeforeExtension(env.EnvironmentName)), optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables(prefix: "DOTNET_")
                        .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                        .AddEnvironmentVariables(prefix: "AWS_")
                        .AddEnvironmentVariables(prefix: "HELLO_");

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }

                    if (env.IsEnvironment(EnvConst.Debug))
                    {
                        config.AddUserSecrets<Program>();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
