namespace Notification.Application.Contracts
{
    /// <summary>
    /// Abstraction for application logging.
    /// </summary>
    /// <typeparam name="T">The type whose name is used for the logger category.</typeparam>
    public interface IAppLogger<T>
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}