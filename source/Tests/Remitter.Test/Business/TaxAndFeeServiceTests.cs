using NSubstitute;
using NUnit.Framework;
using Remitter.Business.Taxes;
using Remitter.Data.External.Providers;
using Remitter.Data.External.Response;
using Remitter.Data.Model;
using Remitter.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Test.Business
{
    [TestFixture]
    public class TaxAndFeeServiceTests
    {
        private ITaxRepository _mockTaxRepository;
        private IFeeRepository _mockFeeRepository;
        private IRemittanceProvider _mockProvider;
        private ITaxAndFeeService _service;
        private CancellationToken _cxlToken = new CancellationTokenSource().Token;

        [SetUp]
        public void Setup()
        {
            _mockFeeRepository = Substitute.For<IFeeRepository>();
            _mockTaxRepository = Substitute.For<ITaxRepository>();
            _mockProvider = Substitute.For<IRemittanceProvider>();

            _service = new TaxAndFeeService(_mockProvider, _mockTaxRepository, _mockFeeRepository);
        }

        #region GetTaxAsync Test

        [Test, Category("GetTaxAsync")]
        public void GetTaxAsync_Should_ThrowException_When_Invalid_CountryCode()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetTaxAsync("", 100, _cxlToken));
        }

        [Test, Category("GetTaxAsync")]
        public void GetTaxAsync_Should_ThrowException_When_Invalid_Amount()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetTaxAsync("US", 0, _cxlToken));
        }

        [Test, Category("GetTaxAsync")]
        public async Task GetTaxAsync_Should_Return_Empty_Tax_When_Not_Present()
        {
            // act
            var result = await _service.GetTaxAsync("US", 100, _cxlToken);

            // assert
            Assert.IsEmpty(result);
        }

        [Test, Category("GetTaxAsync")]
        public async Task GetTaxAsync_Should_Calculate_The_Correct_Tax_Percentage()
        {
            // arrange
            var taxes = new List<Tax> { new Tax { TaxCode = "VAT", TaxName = "Value Added Tax", Percentage = 5.5m } };
            _mockTaxRepository.GetLocalTaxesAsync("US").Returns(Task.FromResult(taxes));

            // act
            var result = await _service.GetTaxAsync("US", 350, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(19.25, result.First().Amount);
            });
        }

        #endregion

        #region GetFeeAsync Test

        [Test, Category("GetFeeAsync")]
        public void GetFeeAsync_Should_Throw_Exception_For_Invalid_FromCountry()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetFeeAsync("", "", 0, _cxlToken));
        }

        [Test, Category("GetFeeAsync")]
        public void GetFeeAsync_Should_Throw_Exception_For_Invalid_ToCountry()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetFeeAsync("US", "", 0, _cxlToken));
        }

        [Test, Category("GetFeeAsync")]
        public void GetFeeAsync_Should_Throw_Exception_For_Invalid_TransferAmount()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetFeeAsync("US", "IN", 0, _cxlToken));
        }

        [Test, Category("GetFeeAsync")]
        public async Task GetFeeAsync_Should_Return_No_Fee__TF_No_Slab()
        {
            // act
            var fees = await _service.GetFeeAsync("US", "IN", 1000, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, fees.Count);
                Assert.AreEqual("TF", fees.First().FeeCode);
            });
        }

        [Test, Category("GetFeeAsync")]
        public async Task GetFeeAsync_Should_Return_No_Fee_TF_No_Matching_Slab()
        {
            // arrange
            var fees = new List<FeeResponse>
            {
                new FeeResponse { Fee = 10, Amount = 1000 },
                new FeeResponse { Fee = 5, Amount = 2000 },
            };
            _mockProvider.GetTransactionFeeAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(fees));

            // act
            var result = await _service.GetFeeAsync("US", "IN", 2001, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("TF", result.First().FeeCode);
                Assert.AreEqual(0, result.First().Amount);
            });
        }

        [Test, Category("GetFeeAsync")]
        public async Task GetFeeAsync_Should_Return_Correct_Fee_1()
        {
            // arrange
            var fees = new List<FeeResponse>
            {
                new FeeResponse { Fee = 20, Amount = 1000 },
                new FeeResponse { Fee = 10, Amount = 2000 },
                new FeeResponse { Fee = 5, Amount = 3000 }
            };
            _mockProvider.GetTransactionFeeAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(fees));

            // act
            var result = await _service.GetFeeAsync("US", "IN", 2500, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("TF", result.First().FeeCode);
                Assert.AreEqual(5, result.First().Amount);
            });
        }

        [Test, Category("GetFeeAsync")]
        public async Task GetFeeAsync_Should_Return_Correct_Fee_2()
        {
            // arrange
            var fees = new List<FeeResponse>
            {
                new FeeResponse { Fee = 20, Amount = 1000 },
                new FeeResponse { Fee = 10, Amount = 2000 },
                new FeeResponse { Fee = 5, Amount = 3000 }
            };
            _mockProvider.GetTransactionFeeAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(fees));

            // act
            var result = await _service.GetFeeAsync("US", "IN", 1500, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("TF", result.First().FeeCode);
                Assert.AreEqual(10, result.First().Amount);
            });
        }

        [Test, Category("GetFeeAsync")]
        public async Task GetFeeAsync_Should_Return_Correct_Fee_3()
        {
            // arrange
            var fees = new List<FeeResponse>
            {
                new FeeResponse { Fee = 20, Amount = 1000 },
                new FeeResponse { Fee = 10, Amount = 2000 },
                new FeeResponse { Fee = 5, Amount = 3000 }
            };
            _mockProvider.GetTransactionFeeAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(fees));

            // act
            var result = await _service.GetFeeAsync("US", "IN", 1, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("TF", result.First().FeeCode);
                Assert.AreEqual(20, result.First().Amount);
            });
        }

        [Test, Category("GetFeeAsync")]
        public async Task GetFeeAsync_Should_Return_Additional_Fee_If_Present()
        {
            // arrange
            var fees = new List<FeeResponse>
            {
                new FeeResponse { Fee = 20, Amount = 1000 },
                new FeeResponse { Fee = 10, Amount = 2000 },
                new FeeResponse { Fee = 5, Amount = 3000 }
            };
            _mockProvider.GetTransactionFeeAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(fees));

            var additionalFees = new List<Fee>
            {
                new Fee { Amount = 100, FeeCode = "CF", FeeName = "Convenience Fee"}
            };
            _mockFeeRepository.GetApplicableFeesAsync("US", "IN")
                .Returns(Task.FromResult(additionalFees));

            // act
            var result = await _service.GetFeeAsync("US", "IN", 1, _cxlToken);

            // assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, result.Count);
                Assert.That(result.Exists(t=> t.FeeCode == "TF"));
                Assert.That(result.Exists(t => t.FeeCode == "CF"));
                Assert.AreEqual(120, result.Sum(t=> t.Amount));
            });
        }

        #endregion
    }
}
