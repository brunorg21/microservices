using Auth.Api.Domain.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Auth.Api.Infra.Cache
{
    public class RedisCacheRepository
        (
            IDistributedCache cache
        ): ICacheRepository
    {
        public async Task<T> GetKeyAsync<T>(string key, CancellationToken ct)
        {
            var bytes = await cache.GetAsync(key, ct);
            if (bytes == null)
                return default!;

            return JsonSerializer.Deserialize<T>(bytes)!;
        }

        public async Task SetKeyAsync<T>(string key, T value, TimeSpan? expirationTime, CancellationToken ct)
        {
            await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            }, ct);
        }
    }
}
