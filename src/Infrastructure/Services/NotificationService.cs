using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Notification.Application.Contracts;
using Notification.Application.Models;
using Notification.ServiceDefaults.Extensions;

namespace Notification.Infrastructure.Services
{
    /// <summary>
    /// Provides an implementation of <see cref="INotificationService"/> that sends notifications using Azure Service Bus.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IOptionsMonitor<ServiceBusSettings> _serviceBusOptions;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _emailTopic;
        private readonly string _smsTopic;

        public NotificationService(
            IOptionsMonitor<ServiceBusSettings> serviceBusOptions,
            ServiceBusClient serviceBusClient)
        {
            _serviceBusOptions = serviceBusOptions ?? throw new ArgumentNullException(nameof(serviceBusOptions));
            _serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));

            var config = _serviceBusOptions.CurrentValue;
            if (config == null)
                throw new ArgumentNullException(nameof(config), "ServiceBusSettings cannot be null.");

            _emailTopic = config.EmailTopicName ?? config.TopicName ?? throw new ArgumentNullException("ServiceBus:EmailTopicName");
            _smsTopic = config.SmsTopicName ?? config.TopicName ?? throw new ArgumentNullException("ServiceBus:SmsTopicName");

            if (string.IsNullOrEmpty(_emailTopic) || string.IsNullOrEmpty(_smsTopic))
                throw new ArgumentNullException("ServiceBus EmailTopicName or ServiceBus:SmsTopicName cannot be null or empty.");
        }

        /// <summary>
        /// Sends an email notification asynchronously.
        /// </summary>
        public async Task EmailNotificationAsync(EmailNotificationRequest request)
        {
            var sender = _serviceBusClient.CreateSender(_emailTopic);
            var message = new ServiceBusMessage(request.ToJson());
            await sender.SendMessageAsync(message);
        }

        /// <summary>
        /// Sends an SMS notification asynchronously.
        /// </summary>
        public async Task SmsNotificationAsync(SmsNotificationRequest request)
        {
            var sender = _serviceBusClient.CreateSender(_smsTopic);
            var message = new ServiceBusMessage(request.ToJson());
            await sender.SendMessageAsync(message);
        }
    }
}