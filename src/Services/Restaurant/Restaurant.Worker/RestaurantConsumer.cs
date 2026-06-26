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
                "Customer with id {CustomerId} has joined on restaurant with id {RestaurantId}",
                @event.CustomerId, @event.RestaurantId);

            await Task.CompletedTask;
        }
    }
}
