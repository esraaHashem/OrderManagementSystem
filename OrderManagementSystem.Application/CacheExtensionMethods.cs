using Microsoft.Extensions.Caching.Memory;
using OrderManagementSystem.Core;
using System.Text.Json;

namespace OrderManagementSystem.Application
{
    public static class CacheExtensionMethods
    {
        public static (bool, TValue?) GetData<TKey, TValue>(CacheParameters<TKey, TValue> cacheParameters, IMemoryCache _cache) where TValue : class
        {
            if (_cache.TryGetValue(cacheParameters.Key, out string? cachedStringAnalytics) && (!string.IsNullOrWhiteSpace(cachedStringAnalytics)))
            {
                var serializedData = JsonSerializer.Deserialize<TValue>(cachedStringAnalytics);

                return (true, serializedData);
            }
            else
            {
                return (false, null);
            }
        }

        public static void SetData<TKey, TValue>(CacheParameters<TKey, TValue> cacheParameters, IMemoryCache _cache)
        {
            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));

            var serializedData = JsonSerializer.Serialize(cacheParameters.Value);

            _cache.Set(cacheParameters.Key, serializedData, cacheOptions);
        }
    }
}