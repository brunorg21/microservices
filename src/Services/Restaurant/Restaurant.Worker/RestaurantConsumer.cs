using Messaging.Shared.Abstractions.Consumers.RabbitMQ;
using Messaging.Shared.Contracts;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;

namespace Restaurant.Worker
{
    public class RestaurantConsumer(
        ILogger<RestaurantConsumer> logger, 
        IRabbitMQConsumer consumer,
        IRegisterQueueEntryUseCase useCase) : BackgroundService
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

            var request = new RegisterQueueEntryRequest
            {
                CustomerId = @event.CustomerId,
                RestaurantId = @event.RestaurantId,
            };

            await useCase.Execute(request);
        }
    }
}
