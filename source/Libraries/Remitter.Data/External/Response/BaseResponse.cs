using System.Collections.Generic;

namespace Remitter.Data.External.Response
{
    public class ClientResponse<T>
    {
        public int StatusCode { get; protected set; }
        public List<string> Errors { get; protected set; }
        public T Result { get; internal set; }

        public ClientResponse()
        {
            Errors = new List<string>();
        }

        public static ClientResponse<T> SuccessResponse(T result) => new ClientResponse<T> { StatusCode = 200, Result = result };
    }
}
