using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Models.Office365.Transition;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Identity.Password;

namespace CloudPlus.Services.Office365.Transition
{
    public class Office365TransitionService : IOffice365TransitionService
    {
        private readonly IOffice365ApiService _office365ApiService;
        private readonly IDomainService _domainService;
        private readonly IOffice356DbOfferService _office356DbOfferService;

        public Office365TransitionService(
            IOffice365ApiService office365ApiService,
            IDomainService domainService,
            IOffice356DbOfferService office356DbOfferService)
        {
            _office365ApiService = office365ApiService;
            _domainService = domainService;
            _office356DbOfferService = office356DbOfferService;
        }

        public async Task<bool> CompanyBelongsToCloudPlusOffice365Async(int companyId)
        {
            var office365CustomerId = await GetOffice365CustomerId(companyId);

            return office365CustomerId != null;
        }

        public async Task<Office365TransitionMatchingDataModel> GetTransitionMatchingDataAsync(int companyId)
        {
            var office365CustomerId = await GetOffice365CustomerId(companyId);
            if (office365CustomerId == null) return null;

            var domain = _domainService.GetCompanyDomains(companyId).FirstOrDefault();
            if (domain == null) return null;

            var offers = await _office356DbOfferService.GetOffice365OffersAsync();

            var data = await _office365ApiService
                .GetTransitionMatchingDataAsync(new Office365CustomerDomainModel
                {
                    Office365CustomerId = office365CustomerId,
                    Domain = domain.Name
                });

            var userPrinciaplNames = data.Select(i => i.UserPrincipalName);
            var office365Domains = userPrinciaplNames.Select(i => i.Split('@').LastOrDefault()).Distinct().Select(s => s).ToList();

            var preparedData = new Office365TransitionMatchingDataModel
            {
                CompanyId = companyId,
                Office365CustomerId = office365CustomerId,
                Domains = office365Domains,
                ProductItems = data.Select(i => new Office365TransitionProductItemModel
                {
                    UserPrincipalName = i.UserPrincipalName,
                    DisplayName = i.DisplayName,
                    Password = PasswordGenerator.Generate(8, PasswordCharacters.AlphaNumeric),
                    CurrentProductItemName = i.CurrentProducts,
                    RecommendedProductItem = new Office365TransitionRecommendedProductItemModel
                    {
                        Name = i.RecommendedProductName,
                        CloudPlusProductIdentifier = offers.FirstOrDefault(o => o.Office365Id == i.RecommendedProductOfferId)?.CloudPlusProductIdentifier
                    },
                    UnsupportedLicensesFound = i.UnsupportedLicensesFound,
                }).ToList()
            };

            return preparedData;
        }

        public async Task<string> GetOffice365CustomerId(int companyId)
        {
            var domains = _domainService.GetCompanyDomains(companyId);

            foreach (var domain in domains)
            {
                var result = await _office365ApiService.GetCustomerIdByDomainAsync(
                    new Office365CustomerDomainModel
                    {
                        Domain = domain.Name
                    });

                if (Guid.TryParse(result, out _))
                    return result;
            }

            return null;
        }
    }
}
