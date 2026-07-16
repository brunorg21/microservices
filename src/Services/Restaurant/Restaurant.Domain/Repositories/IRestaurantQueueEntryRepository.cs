using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories
{
    public interface IRestaurantQueueEntryRepository
    {
        Task<RestaurantQueueEntry> AddAsync(RestaurantQueueEntry restaurantQueueEntry);
        Task<RestaurantQueueEntry?> GetById(Guid id, bool withAsNoTracking = true);
        void Update(RestaurantQueueEntry restaurantQueueEntry);
        Task<int> GetLastPosition();
        Task<RestaurantQueueEntry?> GetNextCustomer(Guid restaurantId); 
    }
}
