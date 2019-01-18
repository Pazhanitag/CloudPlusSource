using System.Collections.Generic;
using CloudPlus.Models.Office365.Domain;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Models.Office365.Customer
{
    public class Office365CustomerModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Office365CustomerId { get; set; }
        public List<Office365DomainModel> Domains { get; set; }
        public List<Office365SubscriptionModel> Office365Subscriptions { get; set; }
    }
}
