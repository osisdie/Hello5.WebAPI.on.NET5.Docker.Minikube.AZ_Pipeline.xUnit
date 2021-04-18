using System;
using CoreFX.Abstractions.Consts;
using CoreFX.Common;
using Microsoft.Extensions.Configuration;
using UnitTest.CoreFX.Hosting.App_Start;
using Xunit;

namespace UnitTest.CoreFX.Hosting.Tests
{
    public class Configuration_Test : DerivedUnitTestBase
    {
        [Fact]
        public void EnvironmentVariables_Test()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Assert.NotNull(env);
            Assert.Equal("Development", EnvConst.Development);
            Assert.True(SvcContext.IsDevelopment());
        }

        [Fact]
        public void AppConfiguration_Test()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Debug.json", optional: false);

            var configuration = builder.Build();

            var logLevel = configuration.GetValue<string>("Logging:LogLevel:Default");
            Assert.Equal("Information", logLevel);
        }

        [Fact]
        public void ConnectionString_Test()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("configs/hellosettings.json", optional: false)
                .AddJsonFile("configs/hellosettings.Debug.json", optional: false);

            var configuration = builder.Build();

            var connString = configuration.GetConnectionString("DefaultConnectionString");
            Assert.NotNull(connString);

            connString = configuration["ConnectionStrings:DefaultConnectionString"];
            Assert.NotNull(connString);
        }
    }
}
