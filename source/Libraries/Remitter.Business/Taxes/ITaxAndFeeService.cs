using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Remitter.Business.Tax;

namespace Remitter.Business.Taxes
{
    public interface ITaxAndFeeService
    {
        /// <summary>
        ///  Gets the taxes for the give country code
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <param name="transferAmount">Amount to transfer</param>
        /// <param name="cxlToken"></param>
        /// <returns>List of taxes</returns>
        Task<List<TaxDto>> GetTaxAsync(string countryCode, decimal transferAmount, CancellationToken cxlToken);

        /// <summary>
        ///  Gets list of fees
        /// </summary>
        /// <param name="fromCountryCode">From country code</param>
        /// <param name="toCountryCode">To country code</param>
        /// <param name="transferAmount">Amount to transfer</param>
        /// <param name="cxlToken"></param>
        /// <returns></returns>
        Task<List<FeeDto>> GetFeeAsync(string fromCountryCode, string toCountryCode, decimal transferAmount, CancellationToken cxlToken);
    }
}
