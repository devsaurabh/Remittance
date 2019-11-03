using Remitter.Data.External.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Business.Location
{
    public interface ICountryService
    {
        /// <summary>
        ///  Gets list of available countries for money remitting
        /// </summary>
        /// <returns></returns>
        Task<IList<CountryDto>> GetSupportedCountriesAsync(CancellationToken cxlToken);

        /// <summary>
        ///  Checks whether the provided countryCode is supported or not
        /// </summary>
        /// <param name="countryCode">From Country code</param>
        /// <param name="toCountryCode">To Country code</param>
        /// <param name="cxlToken"></param>
        /// <returns></returns>
        Task<bool> IsSupportedCountryAsync(string fromCountryCode, string toCountryCode, CancellationToken cxlToken);
    }
}
