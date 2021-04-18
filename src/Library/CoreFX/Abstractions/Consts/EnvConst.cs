namespace CoreFX.Abstractions.Consts
{
    public sealed class EnvConst
    {
        public const string DOTNET_prefix = "DOTNET_";
        public const string ASPNETCORE_prefix = "ASPNETCORE_";
        public const string AWS_prefix = "AWS_";

        public const string Debug = "Debug";
        public const string Testing = "Testing";
        public const string Development = "Development";
        public const string Staging = "Staging";
        public const string Production = "Production";

        public const string MachineName = "MACHINE_NAME";
        public const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        public const string AspNetCoreUrls = "ASPNETCORE_URLS";
        public const string AwsSecretAccessKey = "AWS_SECRET_ACCESS_KEY";
        public const string AwsAccessKeyId = "AWS_ACCESS_KEY_ID";

        public static readonly string SdkApiBearerToken = $"{SvcConst.SdkPrefix.ToUpper()}_API_TOKEN";
        public static readonly string SdkApiKey = $"{SvcConst.SdkPrefix.ToUpper()}_API_KEY";
        public static readonly string SdkApiName = $"{SvcConst.SdkPrefix.ToUpper()}_API_NAME";
        public static readonly string DeploymentName = $"{SvcConst.SdkPrefix.ToUpper()}_DEPLOY_NAME";
    }
}
