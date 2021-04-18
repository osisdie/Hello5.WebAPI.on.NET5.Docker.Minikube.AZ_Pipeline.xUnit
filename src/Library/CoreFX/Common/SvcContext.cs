using System;
using System.IO;
using System.Net;
using System.Reflection;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Enums;

namespace CoreFX.Common
{
    /// <summary>
    /// Global
    /// </summary>
    public class SvcContext : SdkRuntime
    {
        static SvcContext()
        {
            InitialSDK();
        }

        public static void InitialSDK()
        {
            if (!_isInitialized)
            {
                lock (_lock)
                {
                    if (!_isInitialized)
                    {
                        ReadSdkEnvironment();
                        ReadVersionFile();
                        SecurityManage();

                        _isInitialized = true;
                    }
                }
            }
        }

        public static void ReadVersionFile()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SvcConst.DefaultVersionFile);
            try
            {
                if (File.Exists(path))
                {
                    Version = File.ReadAllText(path).Trim();
                }
                else
                {
                    Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                }
            }
            catch { }
        }

        public static void SecurityManage()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.Expect100Continue = false;
        }

        public static void ReadSdkEnvironment()
        {
            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable(EnvConst.AspNetCoreEnvironment)?.Trim();
            if (Enum.TryParse<EnvironmentEnum>(aspNetCoreEnvironment, out var env))
            {
                SdkEnv = env.ToString();
            }
            else
            {
                SdkEnv = EnvironmentEnum.Debug.ToString();
            }

            ApiName = Environment.GetEnvironmentVariable(EnvConst.SdkApiName)?.Trim()?.ToLower()
               ?? throw new ArgumentNullException($"Environment.GetEnvironmentVariable({EnvConst.SdkApiName})");

            #region Optional
            ApiKey = Environment.GetEnvironmentVariable(EnvConst.SdkApiKey)?.Trim();
            DeploymentName = Environment.GetEnvironmentVariable(EnvConst.DeploymentName)?.Trim();

            /// AWS Ordering: 
            /// 1.Environment Variable 
            /// 2.~/.aws/Credentials
            /// 3.ECS container credentials
            /// 4.Instance profile
            AwsAccessKey = Environment.GetEnvironmentVariable(EnvConst.AwsAccessKeyId)?.Trim();
            AwsSecretKey = Environment.GetEnvironmentVariable(EnvConst.AwsSecretAccessKey)?.Trim();
            #endregion
        }

        public static bool IsDevelopment() => string.Equals(SdkEnv, EnvConst.Development, StringComparison.OrdinalIgnoreCase);
        public static bool IsTesting() => string.Equals(SdkEnv, EnvConst.Testing, StringComparison.OrdinalIgnoreCase);
        public static bool IsStaging() => string.Equals(SdkEnv, EnvConst.Staging, StringComparison.OrdinalIgnoreCase);
        public static bool IsProduction() => string.Equals(SdkEnv, EnvConst.Production, StringComparison.OrdinalIgnoreCase);
        public static bool IsDebug() => string.IsNullOrEmpty(SdkEnv) || string.Equals(SdkEnv, EnvConst.Debug, StringComparison.OrdinalIgnoreCase);

        public static string AwsAccessKey;
        public static string AwsSecretKey;
    }
}
