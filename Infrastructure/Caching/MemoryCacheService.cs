using Microsoft.Extensions.Caching.Memory;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.Infrastructure.Caching;

public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        if (cache.TryGetValue(key, out T? value) && value is not null)
        {
            return value;
        }

        value = await factory();
        cache.Set(key, value, expiration ?? TimeSpan.FromMinutes(10));
        return value;
    }

    public void Remove(string key) => cache.Remove(key);
}
