using Remitter.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remitter.Data.Repository
{
    public interface IFeeRepository
    {
        Task<List<Fee>> GetApplicableFeesAsync(string fromCountryCode, string toCountryCode);
    }

    public class MockFeeRepository : IFeeRepository
    {
        public Task<List<Fee>> GetApplicableFeesAsync(string fromCountryCode, string toCountryCode)
        {
            return Task.FromResult(new List<Fee>());
        }
    }
}
