using NSubstitute;
using NUnit.Framework;
using Remitter.Business.Exchange;
using Remitter.Data.External.Providers;
using Remitter.Data.External.Response;
using Remitter.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Business
{
    [TestFixture]
    public class ExchangeServiceTests
    {
        private IRemittanceProvider _mockProvider;
        private IMarkupRepository _mockMarkupRepository;
        private IExchangeService _service;
        private readonly CancellationToken _cxlToken = new CancellationTokenSource().Token;

        [SetUp]
        public void Setup()
        {
            _mockMarkupRepository = Substitute.For<IMarkupRepository>();
            _mockProvider = Substitute.For<IRemittanceProvider>();

            _service = new ExchangeService(_mockProvider, _mockMarkupRepository);
        }

        [Test]
        public void GetExchangeRateAsync_Should_ThrowException_When_Invalid_From_CountryCode()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetExchangeRateAsync("", "IN", _cxlToken));
        }

        [Test]
        public void GetExchangeRateAsync_Should_ThrowException_When_Invalid_To_CountryCode()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetExchangeRateAsync("US", "", _cxlToken));
        }

        [Test]
        public void GetExchangeRateAsync_Should_ThrowException_When_Provider_Fails()
        {
            Assert.ThrowsAsync<Exception>(async () => await _service.GetExchangeRateAsync("US", "IN", _cxlToken));
        }

        [Test]
        public async Task GetExchangeRateAsync_Should_Return_Correct_Exchange_Rate()
        {
            // arrange
            var exchangeRateResponse = new ExchangeRateResponse { DestinationCountry = "IN", SourceCountry = "US", ExchangeRate = 70.10m };
            _mockProvider.GetExchangeRateAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(exchangeRateResponse));

            // act
            var result = await _service.GetExchangeRateAsync("US", "IN", _cxlToken);

            // assert
            Assert.AreEqual(exchangeRateResponse.ExchangeRate, result);
        }

        [Test]
        public async Task GetExchangeRateAsync_Should_Return_Correct_Exchange_Rate_With_Markup()
        {
            // arrange
            var exchangeRateResponse = new ExchangeRateResponse { DestinationCountry = "IN", SourceCountry = "US", ExchangeRate = 1.0m };
            _mockProvider.GetExchangeRateAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(exchangeRateResponse));

            _mockMarkupRepository.GetMarkupPercentageAsync("US", "IN", _cxlToken)
                .Returns(Task.FromResult(5m));

            // act
            var result = await _service.GetExchangeRateAsync("US", "IN", _cxlToken);

            // assert
            Assert.AreEqual(0.95, result);
        }
    }
}
