using Notification.Domain.Enums;
using Notification.Domain.Models;

namespace Notification.Application.Models
{
    /// <summary>
    /// Represents a request to send an SMS notification, including message content, recipients, delivery channel, and optional subject.
    /// </summary>
    public class SmsNotificationRequest : NotificationRequest
    {
        /// <summary>
        /// The message content to be sent via SMS.
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// The recipient details, including user ID and email (if applicable).
        /// </summary>
        public required List<string> Recipients { get; set; }

        /// <summary>
        /// Specifies the delivery method (email, SMS, push notification so on).
        /// </summary>
        public string Channel => NotificationChannel.SMS.ToString();

        /// <summary>
        /// The subject of the sms(optional)
        /// </summary>
        public string? Subject { get; set; }
    }
}
