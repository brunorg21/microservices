using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace Customer.Api.Infra.Cache
{
    public static class CacheHealthCheck
    {
        public static async Task AddCacheHealthCheck(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

                try
                {
                    await cache.SetStringAsync("healthcheck", "OK", new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                    });

                    var value = await cache.GetStringAsync("healthcheck");

                    Log.Information("Cache health check successful: {Value}", value);

                }
                catch (Exception ex)
                {
                    Log.Error($"Cache health check failed: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
