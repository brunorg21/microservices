using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Infra.Database
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<RestaurantQueueEntry> RestaurantQueueEntries { get; set; }
    }
}
