using Notification.Domain.Enums;

namespace Notification.Domain.Models
{
    /// <summary>
    /// Represents the base class for notification requests, containing common properties such as notification ID, correlation ID, and priority.
    /// </summary>
    public abstract class NotificationRequest
    {
        /// <summary>
        ///  A unique identifier for the notification
        /// </summary>
        public required string NotificationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        ///  A unique identifier used to correlate this notification with other operations or requests
        /// </summary>
        public required string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Defines the urgency level (e.g., high, normal, low).
        /// </summary>
        public Priority Priority { get; set; }

    }
}
