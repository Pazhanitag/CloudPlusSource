using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;

namespace CloudPlus.Services.Office365.Domain
{
    public interface IOffice365DomainService
    {
        Task<bool> VerifyCustomerDomain(IOffice365CustomerDomainModel model);
        Task<bool> FederateCustomerDomain(IOffice365CustomerDomainModel model);
        Task<bool> IsDefaultMicrosoftDomainAvailableAsync(string domain);
        Task<bool> IsDomainVerified(IOffice365CustomerDomainModel model);
        Task<bool> IsDomainFederated(IOffice365CustomerDomainModel model);

    }
}
