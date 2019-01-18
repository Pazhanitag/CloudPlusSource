namespace CloudPlus.QueueModels.Office365.AddressValidation
{
    public interface IOffice365AddresValidationRequest
    {
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }

        string City { get; set; }

        string State { get; set; }

        string Country { get; set; }
        string PostalCode { get; set; }

        string PhoneNumber { get; set; }
    }
}
