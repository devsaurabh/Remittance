using Remitter.Data.External.Providers;
using Remitter.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Business.Exchange
{
    public class ExchangeService : IExchangeService
    {
        #region Private Members

        private readonly IRemittanceProvider _provider;
        private readonly IMarkupRepository _markupRepository;

        #endregion

        #region Ctor

        public ExchangeService(IRemittanceProvider provider, IMarkupRepository markupRepository)
        {
            _provider = provider;
            _markupRepository = markupRepository;
        }

        #endregion

        #region Public Methodss

        public async Task<decimal> GetExchangeRateAsync(string fromCountryCode, string toCountryCode, CancellationToken cxlToken)
        {
            if (string.IsNullOrEmpty(fromCountryCode) || string.IsNullOrEmpty(toCountryCode))
            {
                throw new ArgumentNullException("countryCode");
            }

            var exchangeRate = await _provider.GetExchangeRateAsync(fromCountryCode, toCountryCode, cxlToken);

            if (exchangeRate == null)
            {
                throw new Exception("Unable to fetch exchange rate from provider");
            }

            var markup = await _markupRepository.GetMarkupPercentageAsync(fromCountryCode, toCountryCode, cxlToken);
            var markupAmount = exchangeRate.ExchangeRate * markup * 0.01m;
            return exchangeRate.ExchangeRate - markupAmount;
        } 
        
        #endregion
    }
}
