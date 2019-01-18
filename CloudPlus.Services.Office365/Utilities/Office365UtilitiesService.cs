using System;
using System.Threading.Tasks;
using CloudPlus.Services.Office365.Domain;

namespace CloudPlus.Services.Office365.Utilities
{
    public class Office365UtilitiesService : IOffice365UtilitiesService
    {
        private readonly IOffice365DomainService _office365DomainService;

        public Office365UtilitiesService(IOffice365DomainService office365DomainService)
        {
            _office365DomainService = office365DomainService;
        }

        public async Task<string> GenerateDefaultMicrosoftDomainAsync(int companyOu)
        {
            string domain;
            bool domainAvailable;
            var random = new Random();

            do
            {
                // generate random onmicrosoft.com domain
                domain = $"{random.Next(100, 1000)}cldp{companyOu}.onmicrosoft.com";
                domainAvailable = await _office365DomainService.IsDefaultMicrosoftDomainAvailableAsync(domain);

            } while (!domainAvailable);

            return domain;
        }
    }
}
