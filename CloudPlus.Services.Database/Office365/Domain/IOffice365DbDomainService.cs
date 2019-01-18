using System.Threading.Tasks;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Api;

namespace CloudPlus.Services.Database.Office365.Domain
{
    public interface IOffice365DbDomainService
    {
        Task<Office365CustomerDomainModel> CreateDatabaseCustomerDomainAsync(Office365CustomerDomainModel domain);
        Task DeleteDomainAsync(string domain);
        Task ChangeDatabaseCustomerDomainVerifyStatusAsync(string domain, Office365DomainStatus status);
        Task ChangeDatabaseCustomerDomainFederatedStatusAsync(string domain, bool status);
        bool IsAnyDomainAdded(int companyId);
        Task<bool> IsDomainVerified(string domain);
        Task<bool> IsDomainFederated(string domain);
    }
}
