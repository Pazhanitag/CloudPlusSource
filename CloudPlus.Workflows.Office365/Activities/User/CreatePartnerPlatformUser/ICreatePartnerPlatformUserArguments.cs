namespace CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser
{
    public interface ICreatePartnerPlatformUserArguments
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
        string DisplayName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string UsageLocation { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
        string State { get; set; }
        string StreetAddress { get; set; }
        string Password { get; set; }
    }
}
