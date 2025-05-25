using Notification.Application.Models;

namespace Notification.Application.Contracts
{
    /// <summary>
    /// Defines the contract for sending notifications through various channels.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification asynchronously through the email topic.
        /// </summary>
        /// <param name="request">The email notification request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EmailNotificationAsync(EmailNotificationRequest request);

        /// <summary>
        /// Sends a notification asynchronously through the sms topic.
        /// </summary>
        /// <param name="request">The email notification request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SmsNotificationAsync(SmsNotificationRequest request);
    }
}