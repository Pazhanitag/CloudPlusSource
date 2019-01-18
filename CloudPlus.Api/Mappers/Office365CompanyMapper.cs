using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Models.Office365.Api;
using System.Linq;

namespace CloudPlus.Api.Mappers
{
    public static class Office365CompanyMapper
    {
        public static Office365CompanyDomainsViewModel ToOffice365CompanyDomainsViewModel(this Office365CompanyModel company)
        {
            if (company == null)
                return null;

            return new Office365CompanyDomainsViewModel
            {
                CompanyId = company.CompanyId,
                Email = company.Email,
                Domains = company.Domains.Select(d => d.ToOffice365DomainViewModel()),
                Office365CustomerId = company.Office365CustomerId
            };
        }
    }
}