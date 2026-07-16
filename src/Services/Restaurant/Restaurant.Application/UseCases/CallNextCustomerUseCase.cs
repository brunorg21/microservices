using Restaurant.Application.Interfaces;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.UseCases
{
    internal class CallNextCustomerUseCase(
        ILoggedUser loggedUser,
        IRestaurantQueueEntryRepository restaurantQueueRepository,
        IUnitOfWork uow
        ) : ICallNextCustomerUseCase
    {
        public async Task Execute(Guid restaurantQueueEntryId)
        {
            var user = loggedUser.GetLoggedUser();

            var existingEntry = await restaurantQueueRepository.GetById(restaurantQueueEntryId);

            if(existingEntry != null || existingEntry.RestaurantId != user.RestaurantId)
            {
                throw new Exception($"[{nameof(CallNextCustomerUseCase)}] Entry with ID: {restaurantQueueEntryId} not found for your Restaurant");
            }

            var nextCustomer = await restaurantQueueRepository.GetNextCustomer((Guid)user.RestaurantId);

            if (nextCustomer is null)
                throw new Exception($"[{nameof(CallNextCustomerUseCase)}] Next customer not found on queue.");

            nextCustomer.Status = Domain.Enums.RestaurantQueueEntriesStatus.SEATED;
            nextCustomer.SeatedAt = DateTime.UtcNow;

            restaurantQueueRepository.Update(nextCustomer);

            await uow.CommitAsync();
        }
    }
}
