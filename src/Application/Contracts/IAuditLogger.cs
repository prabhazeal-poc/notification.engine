namespace Notification.Application.Contracts
{
    /// <summary>
    /// Abstraction for audit logging.
    /// </summary>
    public interface IAuditLogger
    {
        Task LogAsync(string path, string method, string requestBody, string responseBody, int statusCode, DateTime timestamp);
    }
}