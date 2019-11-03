using System;

namespace Remitter.Data.Model
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CountruCode { get; set; }
        public string StateCode { get; set; }
        public int PostalCode { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
