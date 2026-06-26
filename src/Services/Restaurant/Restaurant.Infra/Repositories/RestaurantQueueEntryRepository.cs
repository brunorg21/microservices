using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;

namespace Restaurant.Infra.Repositories
{
    internal class RestaurantQueueEntryRepository(RestaurantDbContext dbContext) : IRestaurantQueueEntryRepository
    {
        public async Task<RestaurantQueueEntry> AddAsync(RestaurantQueueEntry restaurantQueueEntry)
        {
            var entry = await dbContext.RestaurantQueueEntries.AddAsync(restaurantQueueEntry);

            return entry.Entity;
        }

        public async Task<RestaurantQueueEntry?> GetById(Guid id, bool withAsNoTracking = true)
        {
            var restaurantQueueEntry = await dbContext.RestaurantQueueEntries
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            return restaurantQueueEntry;
        }

        public async Task<int> GetLastPosition()
        {
            return await dbContext.RestaurantQueueEntries.MaxAsync(rq => rq.Position);
        }

        public void Update(RestaurantQueueEntry restaurantQueueEntry)
        {
            dbContext.RestaurantQueueEntries.Update(restaurantQueueEntry);
        }
    }
}
