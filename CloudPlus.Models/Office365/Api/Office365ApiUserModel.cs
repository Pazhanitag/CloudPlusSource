namespace CloudPlus.Models.Office365.Api
{
    public class Office365ApiUserModel
    {
        public string Office365CustomerId { get; set; }
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UsageLocation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string StreetAddress { get; set; }
        public string Password { get; set; }
    }
}
