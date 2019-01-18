using CloudPlus.Api.Office365.Attributes;

namespace CloudPlus.Api.Office365.Models.Domain
{
    public class Office365CustomerDomainModelWithCredentials : IOffice365ModelWithCredentials
    {
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }
        [Office365Name(O365PropertyName = "Domain")]
        public string Domain { get; set; }
        [Office365Name(O365PropertyName = "AdminUserName")]
        public string AdminUserName { get; set; }
        [Office365Name(O365PropertyName = "AdminPassword")]
        public string AdminPassword { get; set; }
    }
}
