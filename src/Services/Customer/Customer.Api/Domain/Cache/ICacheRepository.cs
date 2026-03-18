namespace Customer.Api.Domain.Cache
{
    public interface ICacheRepository
    {
        Task<T> GetAsync<T>(string key, CancellationToken ct);
        Task SetAsync<T>(string key, T value, TimeSpan? expirationTime, CancellationToken ct);
    }
}
