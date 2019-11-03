using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Remitter.Api.ViewModels.Response;

namespace Remitter.Api.Controllers.Secure
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // enable post configuring the token setup
    public class BeneficiaryController : ControllerBase
    {
        /// <summary>
        ///  Gets the list of available beneficiaries
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <param name="userId">User id</param>
        /// <returns>List of Beneficiaries</returns>        
        /// <response code="200"></response>
        [Route("{countryCode}/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpGet]
        public ActionResult<List<BeneficiarySummaryResponse>> Get(string countryCode, int userId)
        {
            // this is only for mocking purpose. Real implementation pending
            List<BeneficiarySummaryResponse> response = new List<BeneficiarySummaryResponse>
            {
                new BeneficiarySummaryResponse { Country = "IN", FullName = "ABC"},
                new BeneficiarySummaryResponse { Country = "US", FullName = "XYZ"}
            };

            return Ok(response.Where(t=> t.Country == countryCode).ToList());
        }
    }
}