using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Infra.Database
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Domain.Entities.Customer> Customers { get; set; } = null!;
    }
}
