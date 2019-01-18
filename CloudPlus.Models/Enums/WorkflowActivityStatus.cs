namespace CloudPlus.Models.Enums
{
    public enum WorkflowActivityStatus
    {
        RoutingSlipStart = 0,
        RoutingSlipCompleted = 1,
        RoutingSlipFaulted = 2,
        ActivityCompleted = 4,
        ActivityFaulted = 5,
        ActivityCompensated = 6,
        ActivityCompensationFailed = 7,
        None = 8
    }
}
