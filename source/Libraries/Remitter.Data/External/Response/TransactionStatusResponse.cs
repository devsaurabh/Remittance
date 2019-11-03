using System;

namespace Remitter.Data.External.Response
{
    public class TransactionStatusResponse
    {
        public Guid TransactionId { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
