using CloudPlus.Enums.Office365;

namespace CloudPlus.Models.Office365.Api
{
    public class Office365CustomerDomainModel : IOffice365CustomerDomainModel
    {
        public string Office365CustomerId { get; set; }
        public string Domain { get; set; }
        public Office365DomainStatus Office365DomainStaus { get; set; }
    }
}
