using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace CoreFX.Logging.Log4net
{
    public class Log4netAdapter : Microsoft.Extensions.Logging.ILogger
    {
        public Log4netAdapter(string loggerName, FileInfo fileInfo)
        {
            var repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy)
            );

            XmlConfigurator.Configure(repository, fileInfo);
            Logger = LogManager.GetLogger(repository.Name, loggerName);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Trace: return Logger.IsDebugEnabled;
                case LogLevel.Critical: return Logger.IsFatalEnabled;
                case LogLevel.Error: return Logger.IsErrorEnabled;
                case LogLevel.Information: return Logger.IsInfoEnabled;
                case LogLevel.Warning: return Logger.IsWarnEnabled;
                default:
                    return false;
            }
        }

        public void Log<T>(
            LogLevel logLevel,
            Microsoft.Extensions.Logging.EventId eventId,
            T state,
            Exception exception,
            Func<T, Exception, string> formatter)
        {
            if (false == IsEnabled(logLevel))
            {
                return;
            }

            if (null == formatter)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = exception?.ToString();
            }

            if (false == string.IsNullOrWhiteSpace(message))
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        Logger.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        Logger.Debug(message);
                        break;
                    case LogLevel.Error:
                        Logger.Error(message);
                        break;
                    case LogLevel.Information:
                        Logger.Info(message);
                        break;
                    case LogLevel.Warning:
                        Logger.Warn(message);
                        break;
                    default:
                        // Silence
                        break;
                }
            }
        }

        public ILog Logger { get; private set; }
    }

}
