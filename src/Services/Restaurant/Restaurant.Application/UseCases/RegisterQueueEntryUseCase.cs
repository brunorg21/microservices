using Medallion.Threading;
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
        IDistributedLockProvider lockProvider
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

                return new RegisterQueueEntryResponse 
                { 
                    QueueEntryId = queueEntry.Id,
                    CustomerId = queueEntry.CustomerId, 
                    RestaurantId = queueEntry.RestaurantId,
                    Position = queueEntry.Position
                };
            }

            throw new Exception($"Failed to register queue entry for Customer with id {request.CustomerId}");
        }
    }
}
