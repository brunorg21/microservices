using Restaurant.Api.Domain.Repositories;
using Restaurant.Api.Infra.Database;

namespace Restaurant.Api.Infra.Repositories
{
    public class RestaurantRepository(RestaurantDbContext dbContext) : IRestaurantRepository
    {
        public async Task<Domain.Entities.Restaurant> CreateAsync(Domain.Entities.Restaurant restaurant)
        {
            var entry = await dbContext.Restaurants.AddAsync(restaurant);

            return entry.Entity;
        }
    }
}
