using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories
{
    public interface IRestaurantTableRepository
    {
        Task<RestaurantTable> AddAsync(RestaurantTable restaurantTable);
    }
}
