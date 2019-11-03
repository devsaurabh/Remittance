using Remitter.Core.Cache;
using System;
using System.Threading.Tasks;

namespace Remitter.Data.Repository
{
    public interface IEstimationRepository
    {
        /// <summary>
        ///  Save the estimation for record keeping and avoid further queries
        /// </summary>
        /// <param name="estimationId"></param>
        /// <param name="estimationJson"></param>
        /// <returns></returns>
        Task SaveEstimationAsync(string estimationId, string estimationJson);

        Task<string> GetEstimationAsync(string estimationId);
    }

    public class CachedEstimationRepository : IEstimationRepository
    {
        private readonly ICacheManager _cacheManager;
        private static readonly TimeSpan CACHE_TTL = TimeSpan.FromMinutes(15);

        public CachedEstimationRepository(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public Task SaveEstimationAsync(string estimationId, string estimationJson)
        {
            string key = $"estimation-{estimationId}";
            return _cacheManager.SetAsync(key, estimationJson, CACHE_TTL);
        }

        public Task<string> GetEstimationAsync(string estimationId)
        {
            string key = $"estimation-{estimationId}";
            return _cacheManager.GetAsync<string>(key);
        }
    }
}
