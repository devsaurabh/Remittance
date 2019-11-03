using Remitter.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remitter.Data.Repository
{
    public interface ITaxRepository
    {
        Task<List<Tax>> GetLocalTaxesAsync(string countryCode);
    }

    public class MockTaxRepository : ITaxRepository
    {
        public Task<List<Tax>> GetLocalTaxesAsync(string countryCode)
        {
            return Task.FromResult(new List<Tax>());
        }
    }
}
