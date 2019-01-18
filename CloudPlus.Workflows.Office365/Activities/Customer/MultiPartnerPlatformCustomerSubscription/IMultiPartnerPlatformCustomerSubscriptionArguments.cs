using System.Collections.Generic;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription
{
    public interface IMultiPartnerPlatformCustomerSubscriptionArguments
    {
        string Office365CustomerId { get; set; }
        IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
    }
}
