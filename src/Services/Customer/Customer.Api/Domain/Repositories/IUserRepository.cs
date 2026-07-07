namespace Auth.Api.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Domain.Entities.User> AddAsync(Domain.Entities.User customer, CancellationToken ct);
    }
}
