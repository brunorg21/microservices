using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Domain.Entities;

namespace Restaurant.Api.Infra.Database
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
    }
}
