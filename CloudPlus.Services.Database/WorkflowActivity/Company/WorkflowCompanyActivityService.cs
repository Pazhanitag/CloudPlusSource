using System.Collections.Generic;
using System.Linq;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Services.Database.WorkflowActivity.Company
{
    public class WorkflowCompanyActivityService : IWorkflowCompanyActivityService
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public WorkflowCompanyActivityService(IWorkflowActivityService workflowActivity)
        {
            _workflowActivityService = workflowActivity;
        }

        // TODO: unit testing
        public bool IsDomainBeingCreated(List<DomainModel> domains)
        {
            return CheckIsDomainBeingCreated(domains);
        }

        // TODO: unit testing
        public bool IsDomainBeingCreated(string domainName)
        {
            var domainsList = new List<DomainModel>
            {
                new DomainModel()
                {
                    Name = domainName
                }
            };

            return CheckIsDomainBeingCreated(domainsList);
        }

        // TODO: unit testing
        private bool CheckIsDomainBeingCreated(List<DomainModel> domains)
        {
            var createCompanyActivities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                {"Data.workflowActivityType", "CreateCompany" },
            }).ToList();

            foreach (var activity in createCompanyActivities)
            {
                foreach (var item in activity.Context.Data)
                {
                    if (item.Key == "domains")
                    {
                        foreach (var domain in domains)
                        {
                            if (item.Value.ToString().Contains($"\"{domain.Name}\""))
                            {
                                var compleatedActivity = createCompanyActivities.FirstOrDefault(a =>
                                    a.Context.TrackingNumber == activity.Context.TrackingNumber &&
                                    a.Context.WorkflowActivityStatus == WorkflowActivityStatus.ActivityCompleted);

                                if (compleatedActivity != null)
                                    return true;

                                return false;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}