using Notification.Domain.Enums;
using Notification.Domain.Models;

namespace Notification.Application.Models
{
    /// <summary>
    /// Represents a request to send an email notification, including recipient details, delivery channel, subject, content, priority, and optional merge tags for personalization.
    /// </summary>
    public class EmailNotificationRequest : NotificationRequest
    {
        /// <summary>
        /// The recipient details, including user ID and email (if applicable).
        /// </summary>
        public required List<string> Recipients { get; set; }

        /// <summary>
        /// Specifies the delivery method (email, SMS, push notification so on).
        /// </summary>
        public string Channel => NotificationChannel.Email.ToString();

        /// <summary>
        /// The subject of the email notification
        /// </summary>
        public required string Subject { get; set; }

        /// <summary>
        /// The actual message body
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Dynamic values to personalize the notification (e.g., order details, timestamps).
        /// </summary>
        public List<string> MergeTags { get; set; } = [];
    }
}
