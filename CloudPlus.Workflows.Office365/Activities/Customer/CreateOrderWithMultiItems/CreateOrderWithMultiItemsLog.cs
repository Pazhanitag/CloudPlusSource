using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems
{
    public class CreateOrderWithMultiItemsLog : ICreateOrderWithMultiItemsLog
    {
        public string Office365CustomerId { get; set; }
        public List<string> Office365Subscriptions { get; set; }
    }
}
