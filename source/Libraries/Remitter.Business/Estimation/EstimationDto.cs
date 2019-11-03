using Newtonsoft.Json;
using Remitter.Business.Payments;
using Remitter.Business.Tax;
using System;
using System.Collections.Generic;

namespace Remitter.Business.Estimation
{
    public class EstimationDto
    {
        public string EstimationId { get; set; }
        public decimal AmountToSend { get; set; }
        public decimal AmountToRecieve { get; set; }
        public decimal ExchangeRate { get; set; }
        public List<PaymentMode> PaymentModes { get; set; }
        public List<FeeDto> Fees { get; set; }
        public List<TaxDto> Taxes { get; set; }
        public DateTime TransferTime { get; set; }

        public static string ToString(EstimationDto estimation)
        {
            return JsonConvert.SerializeObject(estimation);
        }

        public static EstimationDto FromString(string estimationString)
        {
            return JsonConvert.DeserializeObject<EstimationDto>(estimationString);
        }
    }
}
