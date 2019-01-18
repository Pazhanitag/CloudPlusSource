using CloudPlus.Api.Office365.Attributes;

namespace CloudPlus.Api.Office365.Models.User
{
    public class SetUserImmutableIdModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }
        [Office365Name(O365PropertyName = "Upn")]
        public string UserPrincipalName { get; set; }
    }
}
