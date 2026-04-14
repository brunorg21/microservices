using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;

namespace Restaurant.Infra.Repositories
{
    public class RestaurantRepository(RestaurantDbContext dbContext) : IRestaurantRepository
    {
        public async Task<Domain.Entities.Restaurant> AddAsync(Domain.Entities.Restaurant restaurant, CancellationToken ct)
        {
            var entry = await dbContext.Restaurants.AddAsync(restaurant, ct);

            await dbContext.SaveChangesAsync(ct);

            return entry.Entity;
        }
    }
}
