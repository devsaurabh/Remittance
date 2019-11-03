using System;
using System.Threading.Tasks;

namespace Remitter.Core.Cache
{
    public abstract partial class CacheManager : ICacheManager
    {
        public async Task<T> GetOrSetAsync<T>(string cacheKey, TimeSpan ttl, Func<Task<T>> acquire) where T : class
        {
            try
            {
                if (await IsSetAsync(cacheKey))
                {
                    return await GetAsync<T>(cacheKey);
                }

                var result = await acquire();
                if (ttl.Ticks > 0)
                {
                    await SetAsync(cacheKey, result, ttl);
                }
                return result;
            }
            catch (Exception)
            {
                return await acquire();
            }
        }

        public abstract Task<T> GetAsync<T>(string cacheKey) where T : class;

        public abstract Task SetAsync<T>(string key, T result, TimeSpan ttl) where T : class;

        public abstract Task<bool> IsSetAsync(string key);

        public async Task TryRemoveAsync(string key)
        {
            try
            {
                await RemoveAsync(key);
            }
            catch { }
        }

        protected abstract Task RemoveAsync(string key);

        public abstract void Dispose();
    }
}
