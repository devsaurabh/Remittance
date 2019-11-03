using System;
using System.Threading.Tasks;

namespace Remitter.Core.Cache
{
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        ///  Gets/Sets the function result to cache
        /// </summary>
        /// <typeparam name="T">Return time of Function</typeparam>
        /// <param name="cacheKey">CacheKey</param>
        /// <param name="ttl">Time to live</param>
        /// <param name="acquire">Function to get the data</param>
        /// <returns><see cref="{T}"/></returns>
        Task<T> GetOrSetAsync<T>(string cacheKey, TimeSpan ttl, Func<Task<T>> asyncAcquire) where T : class;

        /// <summary>
        ///  Checks whether the key is set or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> IsSetAsync(string key);

        /// <summary>
        ///  Safely tries to remove the key
        /// </summary>
        /// <param name="key"></param>
        Task TryRemoveAsync(string key);

        /// <summary>
        ///  Gets the value from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string cacheKey) where T : class;

        /// <summary>
        ///  Puts the object in cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T result, TimeSpan ttl) where T : class;
    }
}
