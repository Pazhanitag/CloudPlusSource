using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Office365.Operations;

namespace CloudPlus.Services.Office365.Domain
{
    public class Office365DomainService : IOffice365DomainService
    {
        private readonly IPartnerOperations _partnerOperations;
        private readonly IOffice365ApiService _office365ApiService;

        public Office365DomainService(IPartnerOperations partnerOperations, IOffice365ApiService office365ApiService)
        {
            _partnerOperations = partnerOperations;
            _office365ApiService = office365ApiService;
        }

        public async Task<bool> IsDefaultMicrosoftDomainAvailableAsync(string domain)
        {
            return !await _partnerOperations.UserPartnerOperations.Domains.ByDomain(domain).ExistsAsync();
        }

        public async Task<bool> IsDomainVerified(IOffice365CustomerDomainModel model)
        {
            return await _office365ApiService.IsDomainVerified(model);
        }

        public async Task<bool> IsDomainFederated(IOffice365CustomerDomainModel model)
        {
            return await _office365ApiService.IsDomainVerified(model);
        }

        public async Task<bool> VerifyCustomerDomain(IOffice365CustomerDomainModel model)
        {
            var domainVerified = await _office365ApiService.VerifyCustomerDomainAsync(model);

            return domainVerified;
        }

        public async Task<bool> FederateCustomerDomain(IOffice365CustomerDomainModel model)
        {
            var domainFederated = await _office365ApiService.FederateCustomerDomainAsync(model);
            return domainFederated;
        }

    }
}
