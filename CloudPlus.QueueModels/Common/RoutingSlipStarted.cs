using System;
using System.Collections.Generic;
using CloudPlus.Models.Enums;

namespace CloudPlus.QueueModels.Common
{
    public interface IRoutingSlipStarted
    {
        Guid TrackingNumber { get; set; }
        DateTime CreateTimestamp { get; set; }
        IDictionary<string, object> Arguments { get; set; }
        WorkflowActivityType WorkflowActivityType { get; set; }
    }
}
