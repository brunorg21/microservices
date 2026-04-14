namespace Auth.Api.Domain.Cache
{
    public interface ICacheRepository
    {
        Task<T> GetKeyAsync<T>(string key, CancellationToken ct);
        Task SetKeyAsync<T>(string key, T value, TimeSpan? expirationTime, CancellationToken ct);
    }
}
