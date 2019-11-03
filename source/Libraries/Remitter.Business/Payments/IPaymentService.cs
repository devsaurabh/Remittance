using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remitter.Business.Payments
{
    /// <summary>
    ///  This interface represents an payment microservice allowing us
    ///  to accept payments from various mediums like Bank Debit, Credit/Debit etc.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        ///  Gets the list of available payment modes for the country
        /// </summary>
        /// <param name="fromCountryCode">From country code</param>
        /// <returns></returns>
        Task<List<PaymentMode>> GetPaymentModesAsync(string fromCountryCode);

        /// <summary>
        ///  Gets the allowed transaction amount
        /// </summary>
        /// <param name="fromCountryCode">From country code</param>
        /// <returns></returns>
        Task<TransactionAmount> TransactionRangeAsync(string fromCountryCode);
    }
}
