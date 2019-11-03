using Remitter.Data.External.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Data.External.Providers
{
    public interface IRemittanceProvider
    {
        /// <summary>
        /// Gets the list of countries supported by the provider
        /// </summary>
        /// <param name="cxlToken"></param>
        /// <returns><see cref="List{Country}"/></returns>
        Task<List<CountryResponse>> GetCountryListAsync(CancellationToken cxlToken);

        /// <summary>
        ///  Gets the current exchange rate between the provided countries
        /// </summary>
        /// <param name="from">Country From (ISO ALPHA-2)</param>
        /// <param name="to">Country To (ISO ALPHA-2)</param>
        /// <param name="cxlToken"></param>
        /// <returns><see cref="ExchangeRateResponse"/>s</returns>
        Task<ExchangeRateResponse> GetExchangeRateAsync(string from, string to, CancellationToken cxlToken);

        Task<List<FeeResponse>> GetTransactionFeeAsync(string from, string to, CancellationToken cxlToken);

        /// <summary>
        ///  Gets the transaction status
        /// </summary>
        /// <param name="transactionId">Transaction identifier</param>
        /// <param name="cxlToken"></param>
        /// <returns>Current status of transaction</returns>
        Task<TransactionStatusResponse> GetTransactionStatusAsync(Guid transactionId, CancellationToken cxlToken);

        /// <summary>
        ///  Gets list of banks supported in a country
        /// </summary>
        /// <param name="countryCode">Country code (ISO ALPHA-2)</param>
        /// <returns><see cref="List{BankResponse}"/></returns>
        Task<List<BankResponse>> GetBankListAsync(string countryCode);
    }
}
