using NSubstitute;
using NUnit.Framework;
using Remitter.Business.Location;
using Remitter.Data.External.Providers;
using Remitter.Data.External.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Test.Business
{
    [TestFixture]
    public class CountryServiceTests
    {
        private IRemittanceProvider _mockProvider;
        private ICountryService _service;
        private readonly CancellationToken _cxlToken = new CancellationTokenSource().Token;

        [SetUp]
        public void Setup()
        {   
            _mockProvider = Substitute.For<IRemittanceProvider>();

            _service = new CountryService(_mockProvider);
        }

        #region GetSupportedCountriesAsync Tests

        [Test, Category("GetSupportedCountriesAsync")]
        public void GetSupportedCountriesAsync_Should_Throw_Exception_When_Provider_Fails()
        {
            Assert.ThrowsAsync<Exception>(async () => await _service.GetSupportedCountriesAsync(_cxlToken));
        }

        [Test, Category("GetSupportedCountriesAsync")]
        public async Task GetSupportedCountriesAsync_Should_Correct_Country_List()
        {
            // arrange
            List<CountryResponse> countries = new List<CountryResponse>
            {
                new CountryResponse { Code = "US", Name = "United States" },
                new CountryResponse { Code = "IN", Name = "India" },
            };
            _mockProvider.GetCountryListAsync(_cxlToken)
                .Returns(Task.FromResult(countries));

            // act
            var result = await _service.GetSupportedCountriesAsync(_cxlToken);

            // assert
            Assert.AreEqual(countries.Count, result.Count);
        }

        #endregion

        #region IsSupportedCountryAsync Tests

        [Test, Category("IsSupportedCountryAsync")]
        public async Task GetSupportedCountriesAsync_Should_Throw_Exception_From_Country_Is_Blank()
        {
            // act
            var result = await _service.IsSupportedCountryAsync("", "IN", _cxlToken);

            // assert
            Assert.IsFalse(result);
        }

        [Test, Category("IsSupportedCountryAsync")]
        public async Task GetSupportedCountriesAsync_Should_Throw_Exception_To_Country_Is_Blank()
        {
            // act
            var result = await _service.IsSupportedCountryAsync("US", "", _cxlToken);

            // assert
            Assert.IsFalse(result);
        }

        [Test, Category("IsSupportedCountryAsync")]
        public void GetSupportedCountriesAsync_Should_Throw_Exception_When_Provider_Failes()
        {
            Assert.ThrowsAsync<Exception>(async () => await _service.IsSupportedCountryAsync("US", "IN", _cxlToken));
        }

        [Test, Category("IsSupportedCountryAsync")]
        public async Task GetSupportedCountriesAsync_Should_Throw_Exception_From_Country_Is_NotSupported()
        {
            // arrange
            List<CountryResponse> countries = new List<CountryResponse>
            {
                new CountryResponse { Code = "US", Name = "United States" },
                new CountryResponse { Code = "IN", Name = "India" },
            };
            _mockProvider.GetCountryListAsync(_cxlToken)
                .Returns(Task.FromResult(countries));

            // act
            var result = await _service.IsSupportedCountryAsync("TH", "IN", _cxlToken);

            // assert
            Assert.IsFalse(result);
        }

        [Test, Category("IsSupportedCountryAsync")]
        public async Task GetSupportedCountriesAsync_Should_Throw_Exception_To_Country_Is_NotSupported()
        {
            // arrange
            List<CountryResponse> countries = new List<CountryResponse>
            {
                new CountryResponse { Code = "US", Name = "United States" },
                new CountryResponse { Code = "IN", Name = "India" },
            };
            _mockProvider.GetCountryListAsync(_cxlToken)
                .Returns(Task.FromResult(countries));

            // act
            var result = await _service.IsSupportedCountryAsync("US", "TH", _cxlToken);

            // assert
            Assert.IsFalse(result);
        }

        #endregion
    }
}
