using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems
{
    public interface ICreateOrderWithMultiItemsArguments
    {
        Dictionary<string,int> CloudPlusProductIdentifiers { get; set; }
        string Office365CustomerId { get; set; }
    }
}
