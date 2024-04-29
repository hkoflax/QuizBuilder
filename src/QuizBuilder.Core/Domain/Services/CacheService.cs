using Microsoft.Extensions.Caching.Memory;
using QuizBuilder.Core.Domain.Abstractions;

namespace QuizBuilder.Core.Domain.Services
{
    public class CacheService: ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetValue<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void SetValue<T>(string key, T value, TimeSpan expirationTime)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = expirationTime
            };

            _cache.Set(key, value, cacheEntryOptions);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if (_cache.TryGetValue(key, out var cachedValue) && cachedValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }
    }
}
