namespace Remitter.Data.External.Response
{
    public class ExchangeRateResponse
    {
        public string SourceCountry { get; set; }
        public string DestinationCountry { get; set; }
        public decimal ExchangeRate { get; set; }
        public string ExchangeRateToken { get; set; }
    }
}
