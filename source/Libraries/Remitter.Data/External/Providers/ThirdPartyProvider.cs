using Remitter.Core;
using Remitter.Core.Cache;
using Remitter.Data.External.Response;
using RestSharp;
using RestSharp.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Data.External.Providers
{
    public class ThirdPartyProvider : IRemittanceProvider
    {
        private readonly IRestApiHelper _apiHelper;
        private readonly ICacheManager _cacheManager;
        
        public ThirdPartyProvider(IRestApiHelper apiHelper, ICacheManager cacheManager)
        {
            _apiHelper = apiHelper;
            _cacheManager = cacheManager;
        }

        public virtual Task<List<BankResponse>> GetBankListAsync(string countryCode)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<CountryResponse>> GetCountryListAsync(CancellationToken cxlToken)
        {
            var request = new RestRequest(Endpoints.GET_COUNTRIES, Method.POST);
            return _cacheManager.GetOrSetAsync(
                CacheKeys.COUNTRY_LIST,
                CacheKeys.COUNTRY_LIST_TTL,
                () => {
                    return _apiHelper.IdempotentExecuteAsync<List<CountryResponse>>(request, cxlToken);
                });
        }

        public virtual Task<ExchangeRateResponse> GetExchangeRateAsync(string from, string to, CancellationToken cxlToken)
        {   
            Require.Argument("to", to);
            var request = new RestRequest(Endpoints.GET_EXCHANGE_RATE, Method.POST);

            if (!string.IsNullOrEmpty(from)) request.AddParameter("from", from);
            request.AddParameter("to", to);

            return _cacheManager.GetOrSetAsync(
                CacheKeys.EXCHANGE_RATE,
                CacheKeys.EXCHANGE_RATE_TTL,
                () =>
                {
                    return _apiHelper.IdempotentExecuteAsync<ExchangeRateResponse>(request, cxlToken);
                });
            
        }

        public Task<List<FeeResponse>> GetTransactionFeeAsync(string from, string to, CancellationToken cxlToken)
        {
            Require.Argument("to", to);
            var request = new RestRequest(Endpoints.GET_FEES_LIST, Method.POST);

            if (!string.IsNullOrEmpty(from)) request.AddParameter("from", from);
            request.AddParameter("to", to);
            return _apiHelper.IdempotentExecuteAsync<List<FeeResponse>>(request, cxlToken);
        }

        public Task<TransactionStatusResponse> GetTransactionStatusAsync(Guid transactionId, CancellationToken cxlToken)
        {
            Require.Argument("transactionId", transactionId.ToString());
            var request = new RestRequest(Endpoints.GET_TRANSACTION_STATUS, Method.POST);

            return _apiHelper.IdempotentExecuteAsync<TransactionStatusResponse>(request, cxlToken);
        }
    }
}
