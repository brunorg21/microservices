using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;

namespace Restaurant.Infra.Repositories
{
    internal class RestaurantTableRepository(
        RestaurantDbContext dbContext) : IRestaurantTableRepository
    {
        public async Task<RestaurantTable> AddAsync(RestaurantTable restaurantTable)
        {
            var restaurantTableEntity = await dbContext.RestaurantTables.AddAsync(restaurantTable);

            return restaurantTableEntity.Entity;
        }
    }
}
