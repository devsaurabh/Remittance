using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Remitter.Business.Tax;
using Remitter.Data.External.Providers;
using Remitter.Data.Model;
using Remitter.Data.Repository;

namespace Remitter.Business.Taxes
{
    public class TaxAndFeeService : ITaxAndFeeService
    {
        #region Private Members

        private readonly IRemittanceProvider _provider;
        private readonly ITaxRepository _taxRepository;
        private readonly IFeeRepository _feeRepository;

        #endregion

        #region Ctor

        public TaxAndFeeService(IRemittanceProvider provider, ITaxRepository taxRepository, IFeeRepository feeRepository)
        {
            _provider = provider;
            _taxRepository = taxRepository;
            _feeRepository = feeRepository;
        }

        #endregion

        #region Public Methodss

        public async Task<List<FeeDto>> GetFeeAsync(string fromCountryCode, string toCountryCode, decimal transferAmount, CancellationToken cxlToken)
        {
            if (string.IsNullOrEmpty(fromCountryCode) || string.IsNullOrEmpty(toCountryCode))
            {
                throw new ArgumentNullException("countryCode");
            }

            if (transferAmount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            var providerSlab = await _provider.GetTransactionFeeAsync(fromCountryCode, toCountryCode, cxlToken);
            var providerFee = providerSlab?.OrderBy(t => t.Amount).FirstOrDefault(t => transferAmount < t.Amount)?.Fee ?? 0;

            var additionalFees = (await _feeRepository.GetApplicableFeesAsync(fromCountryCode, toCountryCode))
                ?? new List<Fee>();
            additionalFees.Add(new Fee { Amount = providerFee, FeeCode = "TF", FeeName = "Transfer Fee" });

            return additionalFees.Select(t => new FeeDto
            {
                Amount = t.Amount,
                FeeName = t.FeeName,
                FeeCode = t.FeeCode
            }).ToList();
        }

        public async Task<List<TaxDto>> GetTaxAsync(string countryCode, decimal transferAmount, CancellationToken cxlToken)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException("countryCode");
            }

            if (transferAmount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            var taxes = (await _taxRepository.GetLocalTaxesAsync(countryCode))
                ?? new List<Data.Model.Tax>();

            return taxes
                .Select(t => new TaxDto
                {
                    TaxCode = t.TaxCode,
                    TaxName = t.TaxName,
                    Amount = transferAmount * t.Percentage * 0.01m
                }).ToList();
        }

        #endregion
    }
}
