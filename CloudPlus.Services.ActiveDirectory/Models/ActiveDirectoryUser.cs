namespace CloudPlus.Services.ActiveDirectory.Models
{
    public class ActiveDirectoryUser
    {
        public string Upn { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string CompanyOu { get; set; }
        public string CompanyDomain { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CountryCode { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string EmailAddress { get; set; }
    }
}