using System;
using System.Collections.Generic;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.WorkflowActivity
{
    public class WorkflowActivityContentDto
    {
        public Guid TrackingNumber { get; set; }
        public Guid? ExecutionId { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan Duration { get; set; }
        public string ActivityName { get; set; }
        public int? Steps { get; set; }
        public WorkflowActivityStatus WorkflowActivityStatus { get; set; }
        public IDictionary<string, object> Data { get; set; }
        public WorkflowActivityExceptionDto Exception { get; set; }
    }
}