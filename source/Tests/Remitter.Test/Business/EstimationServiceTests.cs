using NSubstitute;
using NUnit.Framework;
using Remitter.Business.Estimation;
using Remitter.Business.Exchange;
using Remitter.Business.Location;
using Remitter.Business.Payments;
using Remitter.Business.Taxes;
using Remitter.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Test.Business
{
    [TestFixture]
    public class EstimationServiceTests
    {
        private ITaxAndFeeService _mockTaxAndFeeService;
        private IPaymentService _mockPaymentService;
        private IExchangeService _mockExchangeService;
        private ICountryService _mockCountryService;
        private IEstimationRepository _mockEstimationRepository;
        private IEstimationService _service;
        private readonly CancellationToken _cxlToken = new CancellationTokenSource().Token;

        [SetUp]
        public void Setup()
        {
            _mockCountryService = Substitute.For<ICountryService>();
            _mockEstimationRepository = Substitute.For<IEstimationRepository>();
            _mockExchangeService = Substitute.For<IExchangeService>();
            _mockPaymentService = Substitute.For<IPaymentService>();
            _mockTaxAndFeeService = Substitute.For<ITaxAndFeeService>();
            _service = new EstimationService(_mockPaymentService, _mockTaxAndFeeService,
                _mockExchangeService, _mockCountryService, _mockEstimationRepository);
        }

        #region GetEstimationAsync Tests

        [Test, Category("GetEstimationAsync")]
        public void GetEstimationAsync_Should_Throw_Exception_When_EstimationId_Not_Cached()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetEstimationAsync("someId"));
        }

        [Test, Category("GetEstimationAsync")]
        public async Task GetEstimationAsync_Should_Return_Cached_Estimation()
        {
            // arrange
            var estimationId = Guid.NewGuid().ToString();
            var estimation = new EstimationDto { EstimationId = estimationId };
            var json = EstimationDto.ToString(estimation);
            _mockEstimationRepository.GetEstimationAsync(estimationId)
                .Returns(json);

            // act
            var result = await _service.GetEstimationAsync(estimationId);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(estimationId, result.EstimationId);
        }

        #endregion

        #region CreateEstimationAsync Tests

        [Test, Category("CreateEstimationAsync")]
        public void CreateEstimationAsync_Should_Throw_Exception_When_Invalid_From_Country()
        {
            // arrange
            _mockCountryService.IsSupportedCountryAsync("", "IN", _cxlToken)
                .Returns(Task.FromResult(false));

            // assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateEstimationAsync("", "IN", 1000, _cxlToken));
        }

        [Test, Category("CreateEstimationAsync")]
        public void CreateEstimationAsync_Should_Throw_Exception_When_Invalid_To_Country()
        {
            // arrange
            _mockCountryService.IsSupportedCountryAsync("US", "", _cxlToken)
                .Returns(Task.FromResult(false));

            // assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateEstimationAsync("US", "", 1000, _cxlToken));
        }

        [Test, Category("CreateEstimationAsync")]
        public void CreateEstimationAsync_Should_Throw_Exception_If_Transfer_Amount_Is_Below_Range()
        {
            // arrange
            _mockCountryService.IsSupportedCountryAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(true));

            _mockPaymentService.TransactionRangeAsync("US")
                .Returns(Task.FromResult(new TransactionAmount { MinAmount = 500, MaxAmount = 1000 }));

            // assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateEstimationAsync("", "IN", 499, _cxlToken));
        }

        [Test, Category("CreateEstimationAsync")]
        public void CreateEstimationAsync_Should_Throw_Exception_If_Transfer_Amount_Is_Above_Range()
        {
            // arrange
            _mockCountryService.IsSupportedCountryAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(true));

            _mockPaymentService.TransactionRangeAsync("US")
                .Returns(Task.FromResult(new TransactionAmount { MinAmount = 500, MaxAmount = 1000 }));

            // assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateEstimationAsync("", "IN", 1001, _cxlToken));
        }

        [Test, Category("CreateEstimationAsync")]
        public async Task CreateEstimationAsync_Should_Return_Correct_Transfer_Amount()
        {
            // arrange
            _mockCountryService.IsSupportedCountryAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(true));

            _mockPaymentService.TransactionRangeAsync("US")
                .Returns(Task.FromResult(new TransactionAmount { MinAmount = 500, MaxAmount = 1000 }));

            //_mockTaxAndFeeService.GetFeeAsync("US", "IN", 600, _cxlToken)
            //    .Returns(Task.FromResult(new List<FeeDto>()));

            _mockExchangeService.GetExchangeRateAsync("US", "IN", _cxlToken)
                .Returns(1);

            // act
            var result = await _service.CreateEstimationAsync("US", "IN", 600, _cxlToken);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.That(!string.IsNullOrEmpty(result.EstimationId));
                Assert.AreEqual(600, result.AmountToRecieve);
            });
        }

        [Test, Category("CreateEstimationAsync")]
        public async Task CreateEstimationAsync_Should_Save_The_Estimation()
        {
            // arrange
            _mockCountryService.IsSupportedCountryAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(true));

            _mockPaymentService.TransactionRangeAsync("US")
                .Returns(Task.FromResult(new TransactionAmount { MinAmount = 500, MaxAmount = 1000 }));

            //_mockTaxAndFeeService.GetFeeAsync("US", "IN", 600, _cxlToken)
            //    .Returns(Task.FromResult(new List<FeeDto>()));

            _mockExchangeService.GetExchangeRateAsync("US", "IN", _cxlToken)
                .Returns(1);

            // act
            var result = await _service.CreateEstimationAsync("US", "IN", 600, _cxlToken);

            await _mockEstimationRepository
                .Received(1)
                .SaveEstimationAsync(result.EstimationId, EstimationDto.ToString(result));
        }

        #endregion
    }
}
