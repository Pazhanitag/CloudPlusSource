using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.Services.Office365.Transition
{
    public interface IOffice365TransitionService
    {
        Task<bool> CompanyBelongsToCloudPlusOffice365Async(int companyId);
        Task<Office365TransitionMatchingDataModel> GetTransitionMatchingDataAsync(int companyId);
        Task<string> GetOffice365CustomerId(int companyId);
    }
}
