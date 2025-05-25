namespace Notification.Application.Models
{
    /// <summary>
    /// Represents the configuration settings required to connect to Azure Service Bus and specify topic names for notifications.
    /// </summary>
    public class ServiceBusSettings
    {
        public required string ConnectionString { get; set; }
        public required string EmailTopicName { get; set; }
        public required string SmsTopicName { get; set; }
        public string? TopicName { get; set; }
    }
}