using CoreFX.Hosting.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CoreFX.Hosting.Extensions
{
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
