using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Data.Repository
{
    public interface IMarkupRepository
    {
        Task<decimal> GetMarkupPercentageAsync(string fromCountryCode, string toCountryCode, CancellationToken cxlToken);
    }

    public class MockMarkupRepository : IMarkupRepository
    {
        public Task<decimal> GetMarkupPercentageAsync(string fromCountryCode, string toCountryCode, CancellationToken cxlToken)
        {
            return Task.FromResult(0m);
        }
    }
}
