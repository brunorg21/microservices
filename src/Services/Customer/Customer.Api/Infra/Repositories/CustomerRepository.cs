using Customer.Api.Domain.Repositories;
using Customer.Api.Infra.Database;

namespace Customer.Api.Infra.Repositories
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
