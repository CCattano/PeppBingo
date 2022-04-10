using Microsoft.Extensions.Caching.Memory;
using System;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Managers.Caches
{
    /// <summary>
    /// Cache manager responsible for holding Twitch API secrets in memory to reduce Db I/O
    /// </summary>
    public interface ITwitchCacheManager
    {
        /// <summary>
        /// Get the Twitch API secrets out of the in-memory 
        /// cache if the they exist and have not expired
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        string GetApiSecret<TKey>(TKey cacheKey) where TKey : Enum;

        /// <summary>
        /// Set the provided Twitch API secrets in the in-memory cache
        /// </summary>
        /// <remarks>
        /// Secrets set in cache are valid for hour from the point of insertion
        /// </remarks>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        void SetApiSecret<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum;
    }

    public class TwitchCacheManager : BaseCacheManager, ITwitchCacheManager
    {
        protected override TimeSpan CacheEntryTTL => TimeSpan.FromHours(1);
        public TwitchCacheManager(IMemoryCache cache) : base(cache)
        {
        }

        public string GetApiSecret<TKey>(TKey cacheKey) where TKey : Enum => base.GetCacheValue(cacheKey);

        public void SetApiSecret<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum => base.SetCacheValue(cacheKey, cacheValue);
    }
}
