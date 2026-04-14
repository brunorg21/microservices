using Messaging.Shared.Abstractions.Consumers.RabbitMQ;
using Messaging.Shared.Contracts;

namespace RestaurantQueue.Worker
{
    public class Worker(ILogger<Worker> logger, IRabbitMQConsumer consumer) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumer.Consume<JoinRestaurantQueueEvent>(HandleMessage);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private Task HandleMessage(JoinRestaurantQueueEvent message)
        {
            logger.LogInformation("Handling message: {Message}", message);
            return Task.CompletedTask;
        }
    }
}
