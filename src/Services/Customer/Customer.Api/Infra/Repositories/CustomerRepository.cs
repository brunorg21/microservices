using Auth.Api.Domain.Repositories;
using Auth.Api.Infra.Database;

namespace Auth.Api.Infra.Repositories
{
    public class CustomerRepository(CustomerDbContext dbContext) : ICustomerRepository
    {
        public async Task<Domain.Entities.Customer> AddAsync(Domain.Entities.Customer customer, CancellationToken ct)
        {
            var result = await dbContext.Customers.AddAsync(customer, ct);

            await dbContext.SaveChangesAsync(ct);

            return result.Entity;
        }
    }
}
