using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Business.Exchange
{
    public interface IExchangeService
    {
        Task<decimal> GetExchangeRateAsync(string fromCountryCode, string toCountryCode, CancellationToken cxlToken);
    }
}
