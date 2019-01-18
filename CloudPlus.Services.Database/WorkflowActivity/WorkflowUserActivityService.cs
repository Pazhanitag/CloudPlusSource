using System.Collections.Generic;
using System.Linq;
using CloudPlus.Models.Enums;

namespace CloudPlus.Services.Database.WorkflowActivity
{
    public class WorkflowUserActivityService : IWorkflowUserActivityService
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public WorkflowUserActivityService(IWorkflowActivityService workflowActivity)
        {
            _workflowActivityService = workflowActivity;
        }

        public bool IsUserBeingCreated(string email)
        {
            var lastUserActivity = _workflowActivityService.Get(new Dictionary<string, object>
            {
                {"Data.email", email },
                {"Data.workflowActivityType", "CreateUser" }
            }).LastOrDefault();

            return lastUserActivity != null &&
                   lastUserActivity.Context.WorkflowActivityStatus == WorkflowActivityStatus.RoutingSlipStart;
        }

        public bool IsUserBeingCreated(string displayName, int companyId)
        {
            var lastUserActivity = _workflowActivityService.Get(new Dictionary<string, object>
            {
                {"Data.displayName", displayName },
                {"Data.companyId", companyId },
                {"Data.workflowActivityType", "CreateUser" }
            }).LastOrDefault();

            return lastUserActivity != null &&
                   lastUserActivity.Context.WorkflowActivityStatus == WorkflowActivityStatus.RoutingSlipStart;
        }
    }
}
