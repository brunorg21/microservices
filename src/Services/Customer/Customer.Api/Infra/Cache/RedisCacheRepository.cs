using Customer.Api.Domain.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace Customer.Api.Infra.Cache
{
    public class RedisCacheRepository
        (
            IDistributedCache cache
        ): ICacheRepository
    {
        public Task<T> GetAsync<T>(string key, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expirationTime, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
