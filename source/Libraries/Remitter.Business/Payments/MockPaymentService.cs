using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remitter.Business.Payments
{
    public class MockPaymentService : IPaymentService
    {
        public Task<List<PaymentMode>> GetPaymentModesAsync(string fromCountryCode)
        {
            List<PaymentMode> modes = new List<PaymentMode>
            {
                new PaymentMode { Code = "DB", Name = "Direct Debit"}
            };
            return Task.FromResult(modes);
        }

        public Task<TransactionAmount> TransactionRangeAsync(string fromCountryCode)
        {
            TransactionAmount range = new TransactionAmount
            {
                MaxAmount = 75000,
                MinAmount = 500
            };
            return Task.FromResult(range);
        }
    }
}
