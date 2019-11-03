using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Remitter.Data.External.Response;

namespace Remitter.Api.Controllers.Secure

{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] //enable this once plug in the authentication server details
    public class TransactionController : ControllerBase
    {
        /// <summary>
        ///  Gets the transaction status for the given transaction id
        /// </summary>
        /// <remarks>
        ///     Sample Request:
        ///     GET /transaction/{id}
        /// </remarks>
        /// <returns>Transaction status</returns>
        /// <response code="200"></response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<ViewModels.Response.TransactionStatusResponse> Get(string id, CancellationToken cxlToken)
        {
            // this is only for mocking purpose. Real implementation pending
            var status = new ViewModels.Response.TransactionStatusResponse { Status = TransactionStatus.Pending.ToString() };
            return Ok(status);
        }
    }
}