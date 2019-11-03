using System;
using System.Collections.Generic;
using System.Text;

namespace Remitter.Business.Exchange
{
    public class ExchangeRate
    {
        public string FromCountryCode { get; set; }
        public string ToCountryCode { get; set; }
        public decimal Rate { get; set; }
        public TimeSpan ValidFor { get; set; }
    }
}
