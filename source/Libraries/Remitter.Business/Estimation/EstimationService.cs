using System.Threading;
using System.Threading.Tasks;
using System;
using Remitter.Business.Payments;
using Remitter.Business.Exchange;
using Remitter.Business.Taxes;
using Remitter.Business.Location;
using Remitter.Data.Repository;

namespace Remitter.Business.Estimation
{
    public class EstimationService : IEstimationService
    {
        #region Private Members

        private readonly ITaxAndFeeService _taxAndFeeService;
        private readonly IPaymentService _paymentService;
        private readonly IExchangeService _exchangeService;
        private readonly ICountryService _countryService;
        private readonly IEstimationRepository _estimationRepository;
        private const int Accuracy = 2;

        #endregion

        #region Ctor

        public EstimationService(IPaymentService paymentService,
            ITaxAndFeeService taxAndFeeService,
            IExchangeService exchangeService,
            ICountryService countryService,
            IEstimationRepository estimationRepository)
        {
            _paymentService = paymentService;
            _taxAndFeeService = taxAndFeeService;
            _exchangeService = exchangeService;
            _countryService = countryService;
            _estimationRepository = estimationRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EstimationDto> GetEstimationAsync(string estimationId)
        {
            var estimation = await _estimationRepository.GetEstimationAsync(estimationId);
            if (string.IsNullOrEmpty(estimation))
            {
                throw new ArgumentException("Invalid estimationId", "estimationId");
            }
            return EstimationDto.FromString(estimation);
        }

        public async Task<EstimationDto> CreateEstimationAsync(string from, string to, decimal transferAmount, CancellationToken cxlToken)
        {
            if (!await _countryService.IsSupportedCountryAsync(from, to, cxlToken))
            {
                throw new ArgumentException("Country not supported");
            }

            TransactionAmount allowedAmount = await _paymentService.TransactionRangeAsync(from);
            if (transferAmount < allowedAmount.MinAmount || transferAmount > allowedAmount.MaxAmount)
            {
                throw new ArgumentException($"Transfer amount must be between {allowedAmount.MinAmount} and {allowedAmount.MaxAmount}");
            }

            var fees = await _taxAndFeeService.GetFeeAsync(from, to, transferAmount, cxlToken);

            var applicableRate = await _exchangeService.GetExchangeRateAsync(from, to, cxlToken);

            var recieveAmount = Math.Round(applicableRate * transferAmount, Accuracy, MidpointRounding.AwayFromZero);

            var applicablePaymentModes = await _paymentService.GetPaymentModesAsync(from);

            var applicableTaxes = await _taxAndFeeService.GetTaxAsync(from, transferAmount, cxlToken);

            var estimation = new EstimationDto
            {
                EstimationId = Guid.NewGuid().ToString(),
                ExchangeRate = applicableRate,
                AmountToSend = transferAmount,
                AmountToRecieve = recieveAmount,
                Fees = fees,
                PaymentModes = applicablePaymentModes,
                Taxes = applicableTaxes
            };
            await _estimationRepository.SaveEstimationAsync(estimation.EstimationId, EstimationDto.ToString(estimation));
            return estimation;
        }
    }

    #endregion
}
