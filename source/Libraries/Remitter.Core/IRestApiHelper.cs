using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Core
{
    public interface IRestApiHelper
    {
        /// <summary>
        ///  Call to external api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="cxlToken"></param>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cxlToken) where T : new();

        /// <summary>
        ///  Idempotent Rest client. This will retry the calls and apply couple of policies for 
        ///  fail safe response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="cxlToken"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        Task<T> IdempotentExecuteAsync<T>(RestRequest request, CancellationToken cxlToken, int retryCount = 3) where T : new();
    }
}


