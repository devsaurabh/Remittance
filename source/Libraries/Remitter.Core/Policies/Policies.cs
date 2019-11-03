using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Remitter.Core.Policies
{
    public static class Policies
    {
        public static AsyncRetryPolicy RetryAsync<TException>(int retryCount = 5) where TException : Exception {
            return Policy
                .Handle<TException>()
                .RetryAsync(retryCount);
        }

        public static AsyncRetryPolicy<IRestResponse<T>> HttpRetry<T>(int retryCount = 3)
        {
            // Handle both exceptions and return values in one policy
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            return Policy
              .Handle<HttpRequestException>()
              .OrResult<IRestResponse<T>>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode) || (int)r.StatusCode == 440)
              .RetryAsync(retryCount);
        }
    }
}
