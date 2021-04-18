using System.IO;

namespace CoreFX.Logging.Log4net
{
    public class Log4netProvider : Microsoft.Extensions.Logging.ILoggerProvider
    {
        public Log4netProvider(string logFilePath)
        {
            ProviderFile = new FileInfo(logFilePath);
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string name)
        {
            return new Log4netAdapter(name, ProviderFile);
        }

        public void Dispose() { }

        public FileInfo ProviderFile { get; private set; }
    }
}
