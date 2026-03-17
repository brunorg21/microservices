namespace Customer.Api.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Domain.Entities.Customer> AddAsync(Domain.Entities.Customer customer, CancellationToken ct);
    }
}
