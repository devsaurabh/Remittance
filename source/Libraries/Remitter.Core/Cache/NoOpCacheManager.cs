using System;
using System.Threading.Tasks;

namespace Remitter.Core.Cache
{
    public class NoOpCacheManager : CacheManager
    {
        public override void Dispose()
        {
            // do nothing
        }

        public override Task<bool> IsSetAsync(string key)
        {
            return Task.FromResult(false);
        }

        public override Task<T> GetAsync<T>(string cacheKey)
        {
            return Task.FromResult(default(T));
        }

        protected override async Task RemoveAsync(string key)
        {
            // do nothing
        }

        public override async Task SetAsync<T>(string key, T result, TimeSpan ttl)
        {
            // do nothing
        }
    }
}
