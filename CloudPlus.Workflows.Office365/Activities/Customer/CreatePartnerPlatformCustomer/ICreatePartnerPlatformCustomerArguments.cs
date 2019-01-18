namespace CloudPlus.Workflows.Office365.Activities.Customer.CreatePartnerPlatformCustomer
{
    public interface ICreatePartnerPlatformCustomerArguments
    {
        int CompanyId { get; set; }
        string Culture { get; set; }
        string Email { get; set; }
        string Language { get; set; }
        string CompanyName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Country { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
    }
}
