using System;
using System.Collections.Generic;
using System.Text;

namespace Remitter.Business.Payments
{
    public class TransactionAmount
    {
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
    }
}
