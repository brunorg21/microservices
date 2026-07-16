using Restaurant.Application.Interfaces;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    internal class CancelQueueEntryUseCase(
        ILoggedUser loggedUser,
        IRestaurantQueueEntryRepository restaurantQueueEntryRepository,
        IUnitOfWork uow
        ) : ICancelQueueEntryUseCase
    {
        public async Task Execute(Guid restaurantQueueEntryId)
        {
            var user = loggedUser.GetLoggedUser();

            var existingQueueEntry = await restaurantQueueEntryRepository.GetById(restaurantQueueEntryId);

            if (existingQueueEntry is null)
                throw new Exception($"[{nameof(CancelQueueEntryUseCase)}] Queue entry not found.");

            existingQueueEntry.Status = Domain.Enums.RestaurantQueueEntriesStatus.CANCELED;

            restaurantQueueEntryRepository.Update(existingQueueEntry);

            await uow.CommitAsync();
        }
    }
}
