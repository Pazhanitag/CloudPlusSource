﻿namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity
{
    public interface IUpdatePartnerPlatformSubscriptionQuantityLog
    {
        string Office365SubscriptionId { get; set; }
        string Office365CustomerId { get; set; }
        int QuantityChange { get; set; }
    }
}