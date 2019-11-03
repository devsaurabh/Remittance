using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace Remitter.Core
{
    public class RestApiHelper : IRestApiHelper
    {
        #region Private Members

        private readonly IRestClient _client;
        private readonly string _accessKey;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public RestApiHelper(ILogger<RestApiHelper> logger, IRestClient client, IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            _client.BaseUrl = new System.Uri(configuration.GetValue<string>("ProviderUrl"));
            _accessKey = configuration.GetValue<string>("ProviderAccessKey");
        }

        #endregion

        #region Public Methods

        public async Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cxlToken) where T : new()
        {
            request.AddParameter("accessKey", _accessKey, ParameterType.UrlSegment);

            var response = await _client.ExecuteTaskAsync<T>(request, cxlToken);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var providerExeption = new HttpException((int)response.StatusCode, message, response.ErrorException);
                _logger.LogError($"Extermal call fail to {_client.BaseUrl}, StatusCode: {response.StatusCode}", providerExeption);
                throw providerExeption;
            }
            return response.Data;
        }

        public async Task<T> IdempotentExecuteAsync<T>(RestRequest request, CancellationToken cxlToken, int retryCount = 3) where T : new()
        {
            request.AddParameter("accessKey", _accessKey, ParameterType.UrlSegment);

            var response = await Policies.Policies.HttpRetry<T>(retryCount)
                .ExecuteAsync(() =>
                _client.ExecuteTaskAsync<T>(request, cxlToken)
                );

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var providerExeption = new HttpException((int)response.StatusCode, message, response.ErrorException);
                _logger.LogError($"Extermal call fail to {_client.BaseUrl}, StatusCode: {response.StatusCode}", providerExeption);
                throw providerExeption;
            }
            return response.Data;
        }

        #endregion
    }
}
