namespace CloudPlus.QueueModels.Office365.AddressValidation
{
    public class Office365AddresValidationRequest : IOffice365AddresValidationRequest
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
