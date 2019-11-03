using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Remitter.Api.ViewModels.Request
{
    public class TransactionStatusRequest
    {
        [BindRequired]
        public string Id { get; set; }
    }
}
