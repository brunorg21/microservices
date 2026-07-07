using Auth.Api.Domain.Repositories;
using Auth.Api.Infra.Database;

namespace Auth.Api.Infra.Repositories
{
    public class UserRepository(AuthDbContext dbContext) : IUserRepository
    {
        public async Task<Domain.Entities.User> AddAsync(Domain.Entities.User customer, CancellationToken ct)
        {
            var result = await dbContext.Users.AddAsync(customer, ct);
            await dbContext.SaveChangesAsync(ct);

            return result.Entity;
        }
    }
}
