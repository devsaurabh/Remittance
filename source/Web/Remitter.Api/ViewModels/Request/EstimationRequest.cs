using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Remitter.Api.ViewModels.Request
{
    public class EstimationRequest
    {   
        [DefaultValue("US")]
        public string From { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Provide the target country")]
        public string To { get; set; }

        [Required(ErrorMessage = "Provide amount to send")]
        public decimal TransferAmount { get; set; }
    }
}
