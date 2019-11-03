using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remitter.Api.ViewModels;
using Remitter.Api.ViewModels.Request;
using Remitter.Business.Estimation;

namespace Remitter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimationController : ControllerBase
    {
        private readonly IEstimationService _estimationService;

        public EstimationController(IEstimationService estimationService)
        {
            _estimationService = estimationService;
        }

        /// <summary>
        ///  Creates a new estimation for the provided request
        /// </summary>
        /// <param name="request"><see cref="EstimationRequest"/> request</param>
        /// <param name="cxlToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<EstimationResponse>> Post(EstimationRequest request, CancellationToken cxlToken)
        {
            var estimation = await _estimationService
                .CreateEstimationAsync(
                request.From,
                request.To,
                request.TransferAmount,
                cxlToken);
            EstimationResponse result = estimation;

            return Created(new Uri($"/estimation/{estimation.EstimationId}", UriKind.Relative), (EstimationResponse)estimation);
        }

        /// <summary>
        ///  Gets the previously created transaction
        /// </summary>
        /// <param name="estimationId">Estimation Id</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<EstimationResponse>> Get(string estimationId)
        {
            var estimation = await _estimationService.GetEstimationAsync(estimationId);
            return Ok((EstimationResponse)estimation);
        }
    }
}