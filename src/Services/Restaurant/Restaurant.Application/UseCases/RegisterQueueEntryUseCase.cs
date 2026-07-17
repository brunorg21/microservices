using Medallion.Threading;
using Messaging.Shared.Abstractions.Publishers.RabbitMQ;
using Messaging.Shared.Contracts;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.DTOs.Requests;
using Restaurant.Domain.DTOs.Responses;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    internal class RegisterQueueEntryUseCase(
        IRestaurantQueueEntryRepository queueEntryRepository,
        IUnitOfWork uow,
        IDistributedLockProvider lockProvider,
        IRabbitMQPublisher publisher
        ) : IRegisterQueueEntryUseCase
    {
        public async Task<RegisterQueueEntryResponse> Execute(RegisterQueueEntryRequest request)
        {
            string lockKey = $"queue:restaurant:{request.RestaurantId}";

            var handle = await lockProvider.TryAcquireLockAsync(
                lockKey,
                timeout: TimeSpan.FromSeconds(5)
                );

            if (handle is null) 
                throw new Exception($"Failed to acquire lock for restaurant {request.RestaurantId}");

            await using(handle)
            {
                var lastPosition = await queueEntryRepository.GetLastPosition();

                var queueToPersist = new RestaurantQueueEntry
                {
                    RestaurantId = request.RestaurantId,
                    CustomerId = request.CustomerId,
                    Position = lastPosition + 1
                };

                var queueEntry = await queueEntryRepository.AddAsync(queueToPersist);
                await uow.CommitAsync();

                var notifyEvent = new NotifyQueueEntryEvent
                {
                    CustomerId = queueEntry.CustomerId,
                    Position = queueEntry.Position
                };

                await publisher.Publish<NotifyQueueEntryEvent>(notifyEvent);

                return new RegisterQueueEntryResponse 
                { 
                    QueueEntryId = queueEntry.Id,
                    CustomerId = queueEntry.CustomerId, 
                    RestaurantId = queueEntry.RestaurantId,
                    Position = queueEntry.Position
                };
            }
        }
    }
}
