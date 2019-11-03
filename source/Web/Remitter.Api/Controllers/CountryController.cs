using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Remitter.Api.ViewModels.Response;
using Remitter.Business.Location;

namespace Remitter.Api.Controllers
{
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        /// <summary>
        ///  Gets the list of countries currently supported for remitting money
        /// </summary>
        /// <remarks>
        ///     Sample Request:
        ///     GET /country
        /// </remarks>
        /// <returns>List of countries</returns>
        /// <response code="200">Returns list of countries</response>
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<ActionResult<List<CountryResponse>>> Post(CancellationToken cxlToken)
        {
            _logger.LogDebug("Some debug");
            var countries = await _countryService.GetSupportedCountriesAsync(cxlToken);
            List<CountryResponse> response = countries.Select(t => (CountryResponse)t).ToList();
            return Ok(response);
        }
    }
}