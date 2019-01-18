using CloudPlus.Models.Domain;
using System.Collections.Generic;

namespace CloudPlus.Services.Database.WorkflowActivity.Company
{
    public interface IWorkflowCompanyActivityService
    {
        bool IsDomainBeingCreated(List<DomainModel> domains);
        bool IsDomainBeingCreated(string domainName);
    }
}
