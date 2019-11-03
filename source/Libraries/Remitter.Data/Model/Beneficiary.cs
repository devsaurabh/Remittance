namespace Remitter.Data.Model
{
    public class Beneficiary
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
        public string BankAccountName { get; set; }
        public long BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}
