using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Remitter.Data.External.Response;

namespace Remitter.Data.External.Providers
{
    public class MockRemittanceProvider : IRemittanceProvider
    {
        public Task<List<BankResponse>> GetBankListAsync(string countryCode)
        {
            throw new NotImplementedException();
        }

        public Task<List<CountryResponse>> GetCountryListAsync(CancellationToken cxlToken)
        {
            return Task.FromResult(new List<CountryResponse> {
                new CountryResponse { Code = "US", Name = "United States" },
                new CountryResponse { Code = "IN", Name = "India"}
            });
        }

        public Task<ExchangeRateResponse> GetExchangeRateAsync(string from, string to, CancellationToken cxlToken)
        {
            List<ExchangeRateResponse> exchangeRates = new List<ExchangeRateResponse>
            {
                new ExchangeRateResponse { SourceCountry = "US", DestinationCountry = "IN", ExchangeRate = 70.70m, ExchangeRateToken = ""},
                new ExchangeRateResponse { SourceCountry = "IN", DestinationCountry = "US", ExchangeRate = 0.014m, ExchangeRateToken = ""}
            };
            return Task.FromResult(exchangeRates.FirstOrDefault(t => t.SourceCountry == from && t.DestinationCountry == to));
        }

        public Task<List<FeeResponse>> GetTransactionFeeAsync(string from, string to, CancellationToken cxlToken)
        {
            List<FeeResponse> fees = new List<FeeResponse>
            {
                new FeeResponse { Amount = 1000, Fee = 100 },
                new FeeResponse { Amount = 5000, Fee = 10 }
            };
            return Task.FromResult(fees);
        }

        public Task<TransactionStatusResponse> GetTransactionStatusAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionStatusResponse> GetTransactionStatusAsync(Guid transactionId, CancellationToken cxlToken)
        {
            throw new NotImplementedException();
        }
    }
}
