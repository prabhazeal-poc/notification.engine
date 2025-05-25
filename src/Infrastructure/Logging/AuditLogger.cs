using Notification.Application.Contracts;

namespace Notification.Infrastructure.Logging
{
    public class AuditLogger : IAuditLogger
    {
        public async Task LogAsync(string path, string method, string requestBody, string responseBody, int statusCode, DateTime timestamp)
        {
            // Persist to a database, file, or external system as needed.
            // For demo, log to console.
            await Task.Run(() =>
            {
                Console.WriteLine($"[Audit Log: ] {timestamp:u} {method} {path} {statusCode}");
                Console.WriteLine($"Request: {requestBody}");
                Console.WriteLine($"Response: {responseBody}");
            });
        }
    }
}