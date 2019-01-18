using CloudPlus.Api.Office365.Attributes;

namespace CloudPlus.Api.Office365.Models.Domain
{
    public class Office365CustomerDomainModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }

        [Office365Name(O365PropertyName = "Domain")]
        public string Domain { get; set; }
    }
}
