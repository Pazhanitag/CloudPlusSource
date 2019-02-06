using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems
{
    public interface ICreateOrderWithMultiItemsLog
    {
        string Office365CustomerId { get; set; }
        List<string> Office365Subscriptions { get; set; }
    }
}
