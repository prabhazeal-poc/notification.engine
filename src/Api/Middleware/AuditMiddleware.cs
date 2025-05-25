using Notification.Application.Contracts;
using System.Text;

namespace Notification.Api.Middleware
{
    /// <summary>
    /// Middleware that captures and logs HTTP request and response payloads for auditing purposes.
    /// Utilizes the IAuditLogger abstraction to persist audit information, supporting Clean Architecture.
    /// </summary>
    public class AuditMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IAuditLogger auditLogger)
        {
            // Capture request body
            context.Request.EnableBuffering();
            string requestBody = "";
            if (context.Request.ContentLength > 0)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // Capture response body
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Log audit
            await auditLogger.LogAsync(
                context.Request.Path,
                context.Request.Method,
                requestBody,
                responseBodyText,
                context.Response.StatusCode,
                DateTime.UtcNow
            );

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    /// <summary>
    /// Extension methods for registering the AuditMiddleware in the application's request pipeline.
    /// </summary>
    public static class AuditMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddleware>();
        }
    }
}