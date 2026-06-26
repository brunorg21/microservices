using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;

namespace Restaurant.Infra.Repositories
{
    public class RestaurantRepository(RestaurantDbContext dbContext) : IRestaurantRepository
    {
        public async Task<Domain.Entities.Restaurant> AddAsync(Domain.Entities.Restaurant restaurant)
        {
            var entry = await dbContext.Restaurants.AddAsync(restaurant);

            await dbContext.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
