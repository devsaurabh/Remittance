using System;
using System.Collections.Generic;
using System.Linq;
using Remitter.Business.Estimation;
using Remitter.Business.Payments;
using Remitter.Business.Tax;

namespace Remitter.Api.ViewModels
{
    public class EstimationResponse
    {
        public string EstimationId { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal RecieveAmount { get; set; }
        public List<FeeDto> TransferFees { get; set; }
        public List<TaxDto> Taxes { get; set; }
        public decimal TotalPayble
        {
            get { return TransferAmount + Taxes.Sum(t => t.Amount) + TransferFees.Sum(t => t.Amount); }
        }
        public DateTime TransferTime { get; set; }
        public List<PaymentMode> PaymentModes { get; set; }

        public static implicit operator EstimationResponse(EstimationDto estimation)
        {
            return new EstimationResponse
            {
                EstimationId = estimation.EstimationId,
                TransferAmount = estimation.AmountToSend,
                RecieveAmount = estimation.AmountToRecieve,
                TransferFees = estimation.Fees,
                Taxes = estimation.Taxes,
                TransferTime = estimation.TransferTime,
                PaymentModes = estimation.PaymentModes
            };
        }
    }
}
