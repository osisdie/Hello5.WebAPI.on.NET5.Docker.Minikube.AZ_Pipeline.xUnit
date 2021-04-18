using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreFX.Abstractions.Logging;
using CoreFX.Abstractions.Logging.Extensions;
using CoreFX.Abstractions.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace CoreFX.Hosting.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogMgr.CreateLogger(GetType());
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            //await _next(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            var conversationSeqId = Guid.NewGuid().ToString();
            context.Items[SvcLoggerKey.ConversationRootId] = conversationSeqId;
            context.Items[SvcLoggerKey.ConversationSeqId] = conversationSeqId;
            context.Request.EnableBuffering();

            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await context.Request.Body.CopyToAsync(requestStream);

                var body = await ReadStreamInChunks(requestStream);
                var message = DefaultLoggerSerializer.Serialize(new Dictionary<string, object>
                {
                    { SvcLoggerKey.Type, "HTTP REQUEST" },
                    { SvcLoggerKey.RequestDto, new Dictionary<string, object>
                        {
                            { SvcLoggerKey.Method, context.Request?.Method },
                            { SvcLoggerKey.HttpScheme, context.Request?.Scheme },
                            { SvcLoggerKey.HttpHost, context.Request?.Host.ToString() },
                            { SvcLoggerKey.HttpPath, context.Request?.Path.ToString() },
                            { SvcLoggerKey.HttpQueryString, context.Request?.QueryString },
                            { SvcLoggerKey.HttpBody, body },
                            { SvcLoggerKey.HttpHeaders, context.Request?.Headers },
                        }
                    },
                    { SvcLoggerKey.ConversationRootId, conversationSeqId },
                    { SvcLoggerKey.ConversationSeqId, conversationSeqId },
                    { SvcLoggerKey.ProgramMethodName, nameof(LogRequest) },
                }.AddDebugData());
                _logger.LogInformation(message);
                context.Request.Body.Position = 0;
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            var conversationRootId = context.Items[SvcLoggerKey.ConversationRootId] ?? "";
            var conversationSeqId = Guid.NewGuid().ToString();
            var originalBodyStream = context.Response.Body;
            using (var responseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var message = DefaultLoggerSerializer.Serialize(new Dictionary<string, object>
                {
                    { SvcLoggerKey.Type, "HTTP RESPONSE" },
                    { SvcLoggerKey.ResponseDto, new Dictionary<string, object>
                        {
                            { SvcLoggerKey.HttpBody, body },
                            { SvcLoggerKey.HttpHeaders, context.Response?.Headers },
                        }
                    },
                    { SvcLoggerKey.RequestDto, new Dictionary<string, object>
                        {
                            { SvcLoggerKey.Method, context.Request?.Method },
                            { SvcLoggerKey.HttpScheme, context.Request?.Scheme },
                            { SvcLoggerKey.HttpHost, context.Request?.Host.ToString() },
                            { SvcLoggerKey.HttpPath, context.Request?.Path.ToString() },
                            { SvcLoggerKey.HttpQueryString, context.Request?.QueryString },
                            { SvcLoggerKey.HttpHeaders, context.Request?.Headers },
                        }
                    },
                    { SvcLoggerKey.ConversationRootId, conversationRootId },
                    { SvcLoggerKey.ConversationSeqId, conversationSeqId },
                    { SvcLoggerKey.ProgramMethodName, nameof(LogResponse) },
                }.AddDebugData());

                _logger.LogInformation(message);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task<string> ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using (var writer = new StringWriter())
            {
                using (var reader = new StreamReader(stream))
                {
                    var readChunk = new char[readChunkBufferLength];
                    int readChunkLength;
                    do
                    {
                        readChunkLength = await reader.ReadBlockAsync(readChunk, 0, readChunkBufferLength);
                        await writer.WriteLineAsync(readChunk, 0, readChunkLength);
                    } while (readChunkLength > 0);

                    return writer.ToString();
                }
            }
        }
    }
}
