using System;
using System.Collections.Generic;
using CoreFX.Abstractions.Configs;
using CoreFX.Abstractions.Serializers;
using CoreFX.Abstractions.Serializers.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreFX.Abstractions.Logging.Extensions
{
    public static class ILogger_Extension
    {
        public static IDictionary<string, T> BeginCollection<T>() =>
           new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public static IDictionary<string, T> TryAdd<T>(this IDictionary<string, T> src, string key, T val)
        {
            src[key] = val;
            return src;
        }

        public static IDictionary<string, T> AddDebugData<T>(this IDictionary<string, T> src)
        {
            src[SvcLoggerKey.Version] = (T)Convert.ChangeType(SdkRuntime.Version, typeof(T));
            src[SvcLoggerKey.ApiName] = (T)Convert.ChangeType(SdkRuntime.ApiName, typeof(T));
            src[SvcLoggerKey.Deployment] = (T)Convert.ChangeType(SdkRuntime.DeploymentName, typeof(T));
            src[SvcLoggerKey.Environment] = (T)Convert.ChangeType(SdkRuntime.SdkEnv, typeof(T));
            src[SvcLoggerKey.LocalIP] = (T)Convert.ChangeType(SdkRuntime.LocalIP, typeof(T));
            src[SvcLoggerKey.HostName] = (T)Convert.ChangeType(SdkRuntime.MachineName, typeof(T));
            src[SvcLoggerKey.Platform] = (T)Convert.ChangeType(Environment.OSVersion.Platform.ToString(), typeof(T));
            src[SvcLoggerKey.TimeStamp] = (T)Convert.ChangeType(DateTime.UtcNow.ToString("s"), typeof(T));
            src[SvcLoggerKey.UpTime] = (T)Convert.ChangeType(SdkRuntime._ts.ToString("s"), typeof(T));

            return src;
        }

        public static void LogDebugJson(this ILogger logger, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Debug, exception, message, args);
        }

        public static void LogDebugJson(this ILogger logger, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Debug, message, args);
        }

        public static void LogTraceJson(this ILogger logger, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Trace, exception, message, args);
        }

        public static void LogTraceJson(this ILogger logger, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Trace, message, args);
        }

        public static void LogInformationJson(this ILogger logger, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Information, exception, message, args);
        }

        public static void LogInformationJson(this ILogger logger, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Information, message, args);
        }

        public static void LogWarningJson(this ILogger logger, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Warning, exception, message, args);
        }

        public static void LogWarningJson(this ILogger logger, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Warning, message, args);
        }

        public static void LogErrorJson(this ILogger logger, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Error, exception, message, args);
        }

        public static void LogErrorJson(this ILogger logger, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Error, message, args);
        }

        public static void LogCriticalJson(this ILogger logger, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Critical, exception, message, args);
        }

        public static void LogCriticalJson(this ILogger logger, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(LogLevel.Critical, message, args);
        }

        public static void LogJson(this ILogger logger, LogLevel logLevel, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(logLevel, 0, null, message, args);
        }

        public static void LogJson(this ILogger logger, LogLevel logLevel, Exception exception, object body, ISerializer serializer, params object[] args)
        {
            var message = serializer != null ? serializer.Serialize(body) : DefaultLoggerSerializer.Serialize(body);
            logger.Log(logLevel, 0, exception, message, args);
        }
    }
}
