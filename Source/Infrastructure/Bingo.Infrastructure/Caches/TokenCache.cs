using Microsoft.Extensions.Caching.Memory;
using System;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Caches
{
    /// <summary>
    /// Cache responsible for holding JWT signing secrets in memory to reduce Db I/O
    /// </summary>
    public interface ITokenCache
    {
        /// <summary>
        /// Get the Token signing secrets out of the in-memory 
        /// cache if the they exist and have not expired
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        string GetTokenSecret<TKey>(TKey cacheKey) where TKey : Enum;
        /// <summary>
        /// Set the provided API signing secret in the in-memory cache
        /// </summary>
        /// <remarks>
        /// Secrets set in cache are valid for 1 day from the point of insertion
        /// </remarks>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        void SetTokenSecret<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum;
    }

    /// <<inheritdoc cref="ITokenCache"/>
    public class TokenCache : BaseCache, ITokenCache
    {
        protected override TimeSpan CacheEntryTTL => TimeSpan.FromDays(1);
        public TokenCache(IMemoryCache cache) : base(cache)
        {
        }

        public string GetTokenSecret<TKey>(TKey cacheKey) where TKey : Enum => base.GetCacheValue(cacheKey);

        public void SetTokenSecret<TKey>(TKey cacheKey, string cacheValue) where TKey : Enum => base.SetCacheValue(cacheKey, cacheValue);
    }
}
