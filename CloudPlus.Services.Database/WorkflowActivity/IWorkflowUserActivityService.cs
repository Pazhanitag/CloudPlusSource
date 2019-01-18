namespace CloudPlus.Services.Database.WorkflowActivity
{
    public interface IWorkflowUserActivityService
    {
        bool IsUserBeingCreated(string email);
        bool IsUserBeingCreated(string displayName, int companyId);
    }
}