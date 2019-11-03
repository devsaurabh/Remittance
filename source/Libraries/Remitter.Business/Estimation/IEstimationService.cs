using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Business.Estimation
{
    public interface IEstimationService
    {
        /// <summary>
        ///  Gets the estimation for the provided estimation Id
        /// </summary>
        /// <param name="estimationId"></param>
        /// <returns></returns>
        Task<EstimationDto> GetEstimationAsync(string estimationId);

        /// <summary>
        ///  Creates the estimation for transfer amount including fees
        /// </summary>
        /// <param name="from">Country to tranfer from</param>
        /// <param name="to">Country to transfer to</param>
        /// <param name="transferAmount">Amount to transfer</param>
        /// <param name="cxlToken">Cancellation token</param>
        /// <returns><see cref="EstimationDto"/></returns>
        Task<EstimationDto> CreateEstimationAsync(string from, string to, decimal transferAmount, CancellationToken cxlToken);
    }
}
