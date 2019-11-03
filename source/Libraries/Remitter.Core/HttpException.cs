using System;

namespace Remitter.Core
{
    public class HttpException : Exception
    {
        public int StatusCode { get; }
        public HttpException(int statusCode, string message, Exception innerException): base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
