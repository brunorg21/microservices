using Restaurant.Api.Domain.Repositories;
using Restaurant.Api.Infra.Database;

namespace Restaurant.Api.Infra.Repositories
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
