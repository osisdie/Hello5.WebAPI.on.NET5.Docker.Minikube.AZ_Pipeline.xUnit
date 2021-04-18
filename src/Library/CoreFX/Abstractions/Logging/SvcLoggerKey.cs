namespace CoreFX.Abstractions.Logging
{
    public sealed class SvcLoggerKey
    {
        // Common
        public const string ElapsedSeconds = "elapsed-secs";
        public const string ApiKey = "api-key";
        public const string ApiName = "api-name";
        public const string Service = "svc";
        public const string Deployment = "deploy";
        public const string Version = "ver";
        public const string Environment = "env";
        public const string LocalIP = "_ip";
        public const string HostName = "_host";
        public const string Platform = "_os";
        public const string TimeStamp = "_ts";
        public const string UpTime = "_up";

        // Foundation
        public const string Provider = "provider";
        public const string CacheKey = "cache-key";
        public const string Caller = "caller";
        public const string SrcFilePath = "src-file-path";
        public const string IsTimeout = "isTimeout";
        public const string Database = "database";
        public const string ConnectionGuid = "connection-guid";
        public const string ConnectionTimeout = "connection-timeout";
        public const string CommandTimeout = "command-timeout";
        public const string CommandText = "command-text";
        public const string CommandType = "command-type";
        public const string CommandGuid = "command-guid";
        public const string Paramters = "paramters";
        public const string FilePattern = "file-pattern";
        public const string Files = "files";
        public const string Path = "path";
        public const string Insight = "inspection";
        public const string ProgramMethodName = "program-method";

        // Communcation
        public const string Type = "type";
        public const string Method = "method";
        public const string Phase = "phase";
        public const string HttpScheme = "scheme";
        public const string HttpHost = "host";
        public const string HttpPath = "path";
        public const string HttpQueryString = "querystring";
        public const string HttpBody = "body";
        public const string HttpHeaders = "headers";
        public const string Headers = "header";
        public const string Contract = "contract";
        public const string Requester = "requester";
        public const string AccountId = "account-id";
        public const string AccountLevel = "account-level";
        public const string ResponseDto = "response";
        public const string RequestDto = "request";
        public const string Url = "url";
        public const string ClientIP = "client-ip";
        public const string Exception = "exception";
        public const string ConversationRootId = "conversation-root-id";
        public const string ConversationSeqId = "conversation-seq-id";

        // Timestamp
        public const string MillisecondsDuration = "ms";
    }
}
