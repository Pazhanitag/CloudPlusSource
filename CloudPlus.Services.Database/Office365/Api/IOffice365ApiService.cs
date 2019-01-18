using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Models.Office365.Transition;
using CloudPlus.Models.Office365.User;

namespace CloudPlus.Services.Database.Office365.Api
{
    public interface IOffice365ApiService
    {
        Task AddCustomerDomainAsync(IOffice365CustomerDomainModel model);
        Task<string> GetCustomerIdByDomainAsync(IOffice365CustomerDomainModel model);
        Task<string> GetCustomerDomainTxtRecordsAsync(IOffice365CustomerDomainModel model);
        Task RemoveCustomerDomainAsync(IOffice365CustomerDomainModel model);
        Task<bool> VerifyCustomerDomainAsync(IOffice365CustomerDomainModel model);
        Task<bool> FederateCustomerDomainAsync(IOffice365CustomerDomainModel model);
        Task<string> CreateOffice365UserAsync(Office365ApiUserModel model);
        Task<List<string>> GetUserRoles(Office365UserRolesModel model);
        Task AssingUserRoles(Office365UserRolesModel model);
        Task RemoveUserRoles(Office365UserRolesModel model);
        Task<bool> IsDomainVerified(IOffice365CustomerDomainModel model);
        Task<bool> IsDomainfederated(IOffice365CustomerDomainModel model);
        Task UserHardDeleteAsync(Office365ApiUserModel model);
        Task<List<Office365TransitionBasicMatchingDataModel>> GetTransitionMatchingDataAsync(IOffice365CustomerDomainModel model);
        Task<bool> SetImmutableId(Office365ImmutableIdModel model);
    }
}
