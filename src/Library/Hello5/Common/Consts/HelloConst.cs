namespace Hello5.Domain.Common.Consts
{
    public sealed class HelloConst
    {
        public const string DefaultConfigFile = "hellosettings.json"; // lowercase
        public const string DefaultSectionName = "HELLO";
        public const string DefaultDatabaseConnectionKey = "HELLODB_CONN";
        public const string DefaultCacheConnectionKey = "REDIS_CACHE_CONN";
        public const string EchoCommand = "ECHO";
    }
}
