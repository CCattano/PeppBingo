using Microsoft.Extensions.Caching.Memory;
using System;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Managers.Caches
{
    public interface ITwitchCacheManager
    {
        /// <summary>
        /// Get the Twitch API secrets out of the in-memory 
        /// cache if the they exist and have not expired
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetApiSecret<TKey>(TKey cacheKey) where TKey : Enum;
        /// <summary>
        /// Set the provided Twitch API secrets in the in-memory cache
        /// </summary>
        /// <remarks>
        /// Secrets set in cache are valid for hour from the point of insertion
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiSecrets"></param>
        void SetApiSecret<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum;
    }

    public class TwitchCacheManager : ITwitchCacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheEntryTTL = TimeSpan.FromHours(1);
        public TwitchCacheManager(IMemoryCache cache) => _cache = cache;

        public string GetApiSecret<TKey>(TKey cacheKey) where TKey : Enum =>
            _cache.TryGetValue<string>(Enum.GetName(typeof(TKey), cacheKey), out string result)
                ? result
                : null;

        public void SetApiSecret<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum =>
            _cache.Set<string>(Enum.GetName(typeof(TKey), cacheKey), cacheValue, _cacheEntryTTL);
    }
}
