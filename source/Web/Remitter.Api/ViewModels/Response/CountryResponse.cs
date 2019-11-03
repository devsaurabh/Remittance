using Remitter.Business.Location;
using System.ComponentModel;

namespace Remitter.Api.ViewModels.Response
{
    public class CountryResponse
    {
        [Description("Name of the country")]
        public string Name { get; set; }

        [Description("Country Code (ISO ALPHA-2)")]
        public string Code { get; set; }

        public static implicit operator CountryResponse(CountryDto country)
        {
            return new CountryResponse { Code = country.CountryCode, Name = country.Name };
        }
    }

    public class TransactionStatusResponse
    {
        public string Status { get; set; }
    }
}
