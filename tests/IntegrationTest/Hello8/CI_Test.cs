using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Serializers;
using CoreFX.Common;
using Hello8.Domain.Contract.Models.Echo;
using IntegrationTest.Hello8.App_Start;
using TestAbstractions.Consts;
using Xunit;

namespace IntegrationTest.Hello8
{
    public class CI_Test : DerivedUnitTestBase
    {
        private readonly string _endpointHostUrl = "http://localhost:5005";

        public CI_Test()
        {
            var endpointHostUrl = Environment.GetEnvironmentVariable(TestConst.DefaultEndpointKey);
            if (!string.IsNullOrEmpty(endpointHostUrl))
            {
                _endpointHostUrl = endpointHostUrl;
            }
        }

        [Fact(Skip = "Test if endpoint is accessible")]
        public async Task Integration_Test()
        {
            var uri = new Uri(_endpointHostUrl);
            using (var httpClient = new HttpClient() { BaseAddress = uri })
            {
                // check if endpoint is accessible
                {
                    var res = await httpClient.GetAsync("/health");
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);

                    var content = await res.Content.ReadAsStringAsync();
                    Assert.Equal(SvcConst.DefaultHealthyResponse, content);
                }

                // version number
                {
                    var res = await httpClient.GetAsync("/api/echo/version");
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);

                    var content = await res.Content.ReadAsStringAsync();
                    Assert.NotNull(content);
                    var respDto = DefaultJsonSerializer.Deserialize<HelloEchoVersionResponseDto>(content);
                    Assert.True(respDto?.IsSuccess);
                    Assert.NotNull(respDto.Data);
                    Assert.Contains(new Version(1, 0, 0).ToString(), respDto.Data);
                }

                // config
                {
                    var res = await httpClient.GetAsync("/api/echo/config");
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);

                    var content = await res.Content.ReadAsStringAsync();
                    Assert.NotNull(content);
                    var respDto = DefaultJsonSerializer.Deserialize<HelloEchoConfigResponseDto>(content);
                    Assert.True(respDto?.IsSuccess);
                    Assert.NotEmpty(respDto.Data);
                }

                // db
                {
                    var res = await httpClient.GetAsync("/api/echo/db");
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);

                    var content = await res.Content.ReadAsStringAsync();
                    Assert.NotNull(content);
                    var respDto = DefaultJsonSerializer.Deserialize<HelloEchoDBResponseDto>(content);
                    Assert.True(respDto?.IsSuccess);
                    Assert.NotNull(respDto.Data);
                }

                // cache
                {
                    var res = await httpClient.GetAsync("/api/echo/cache");
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);

                    var content = await res.Content.ReadAsStringAsync();
                    Assert.NotNull(content);
                    var respDto = DefaultJsonSerializer.Deserialize<HelloEchoCacheResponseDto>(content);
                    Assert.True(respDto?.IsSuccess);
                    Assert.NotNull(respDto.Data);
                }

                // dump
                {
                    var res = await httpClient.GetAsync("/api/echo/dump");
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);

                    var content = await res.Content.ReadAsStringAsync();
                    Assert.NotNull(content);
                    var respDto = DefaultJsonSerializer.Deserialize<HelloEchoDumpResponseDto>(content);
                    Assert.True(respDto?.IsSuccess);
                    Assert.NotEmpty(respDto.ExtMap);
                }
            }
        }

        [Fact]
        public void EnvironmentVariable_Test()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Assert.NotNull(env);
            Assert.Equal("Development", EnvConst.Development);
            Assert.True(SvcContext.IsDevelopment());
            Assert.Equal(env, SdkRuntime.SdkEnv);
        }
    }
}
