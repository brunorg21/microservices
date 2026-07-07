using Auth.Api.Domain.Entities;

namespace Auth.Api.Domain.Security.Token
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(User user);
    }
}
