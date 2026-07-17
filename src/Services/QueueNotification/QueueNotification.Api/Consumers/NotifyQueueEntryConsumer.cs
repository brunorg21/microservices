
using Messaging.Shared.Abstractions.Consumers.RabbitMQ;

namespace QueueNotification.Api.Consumers
{
    public class NotifyQueueEntryConsumer(
        IRabbitMQConsumer consumer
        ) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
