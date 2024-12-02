using Microsoft.Extensions.Caching.Memory;


namespace MQ.Finder.Data.Finder.Cache
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        private readonly IMemoryCache _cache = memoryCache;

        public T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            _cache.Set(key, value, cacheEntryOptions);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
