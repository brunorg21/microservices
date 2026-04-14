using Messaging.Shared.Abstractions.Consumers.RabbitMQ;
using Messaging.Shared.Contracts;

namespace Restaurant.Worker
{
    public class RestaurantConsumer(
        ILogger<RestaurantConsumer> logger, 
        IRabbitMQConsumer consumer) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumer.Consume<JoinRestaurantQueueEvent>(HandleJoinQueueAsync);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleJoinQueueAsync(JoinRestaurantQueueEvent @event)
        {
            logger.LogInformation(
                "Customer with id {CustomerId} and name {CustomerName} has joined the restaurant queue.",
                @event.CustomerId, @event.CustomerName);

            await Task.CompletedTask;
        }
    }
}
