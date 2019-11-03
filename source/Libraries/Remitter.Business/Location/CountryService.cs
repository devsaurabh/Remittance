using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Remitter.Data.External.Providers;

namespace Remitter.Business.Location
{
    public class CountryService : ICountryService
    {
        #region Private Members

        private readonly IRemittanceProvider _dataProvider;

        #endregion

        #region Ctor

        public CountryService(IRemittanceProvider provider)
        {
            _dataProvider = provider;
        }

        #endregion

        #region Public Methods

        public async Task<IList<CountryDto>> GetSupportedCountriesAsync(CancellationToken cxlToken)
        {
            var response = await _dataProvider.GetCountryListAsync(cxlToken);
            if (response == null)
            {
                throw new Exception("Provider Exception");
            }
            return response.Select(t => new CountryDto { CountryCode = t.Code, Name = t.Name }).ToList();
        }

        public async Task<bool> IsSupportedCountryAsync(string fromCountryCode, string toCountryCode, CancellationToken cxlToken)
        {
            if (string.IsNullOrEmpty(fromCountryCode) || string.IsNullOrEmpty(toCountryCode))
            {
                return false;
            }

            var response = await _dataProvider.GetCountryListAsync(cxlToken);
            if (response == null)
            {
                throw new Exception("Provider Exception");
            }

            return response.Exists(t => t.Code == fromCountryCode) && response.Exists(t => t.Code == toCountryCode);
        }

        #endregion
    }
}
    