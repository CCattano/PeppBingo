using Microsoft.Extensions.Caching.Memory;
using System;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Caches
{
    public abstract class BaseCache
    {
        private readonly IMemoryCache _cache;
        protected abstract TimeSpan CacheEntryTTL { get; }

        public BaseCache(IMemoryCache cache) => _cache = cache;

        protected string GetCacheValue<TKey>(TKey cacheKey) where TKey : Enum =>
            _cache.TryGetValue<string>(Enum.GetName(typeof(TKey), cacheKey), out string result)
                ? result
                : null;

        protected void SetCacheValue<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum =>
            _cache.Set<string>(Enum.GetName(typeof(TKey), cacheKey), cacheValue, CacheEntryTTL);
    }
}
