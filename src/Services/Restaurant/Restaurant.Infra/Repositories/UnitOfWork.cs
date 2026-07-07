using Restaurant.Domain.Repositories;
using Restaurant.Infra.Database;

namespace Restaurant.Infra.Repositories
{
    internal class UnitOfWork(
        RestaurantDbContext restaurantDbContext
        ) : IUnitOfWork
    {
        public async Task CommitAsync()
        {
            await restaurantDbContext.SaveChangesAsync();
        }
    }
}
