using System.Threading.Tasks;
using CloudPlus.QueueModels.Companies.Commands;
using MassTransit;
using CloudPlus.Services.Database.Company;
using CloudPlus.Models.Company;
using CloudPlus.Models.Enums;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Identity.Client;

namespace CloudPlus.AppServices.Company.Consumers
{
    public class UpdateCompanyConsumer : IUpdateCompanyConsumer
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyCatalogService _companyCatalogService;
        private readonly IClientService _clientService;

        public UpdateCompanyConsumer(
            ICompanyService companyService,
            ICompanyCatalogService companyCatalogService,
            IClientService clientService)
        {
            _companyService = companyService;
            _companyCatalogService = companyCatalogService;
            _clientService = clientService;
        }

        public async Task Consume(ConsumeContext<IUpdateCompanyCommand> context)
        {
            var companyArguments = context.Message;

            var companyModel = Map(companyArguments);

            var company = _companyService.GetCompany(companyArguments.CompanyId);
            if (company?.Type == CompanyType.Reseller)
            {
                var oldUri = $"http://{company.ControlPanelSiteUrl}/static/callback.html";
                var newUri = $"http://{companyArguments.ControlPanelSiteUrl}/static/callback.html";
                var oldSilentUri = $"http://{company.ControlPanelSiteUrl}/static/silent.html";
                var newSilentUri = $"http://{companyArguments.ControlPanelSiteUrl}/static/silent.html";

                await _clientService.UpdateRedirectUri(oldUri, newUri);
                await _clientService.UpdateRedirectUri(oldSilentUri, newSilentUri);
                await _clientService.UpdatePostLogoutRedirectUri(company.ControlPanelSiteUrl,
                    $"http://{companyArguments.ControlPanelSiteUrl}");
            }

            await _companyService.UpdateAsync(companyModel);

            if (companyModel.ParentId.HasValue && companyModel.CatalogId.HasValue)
            {
                await _companyCatalogService.AssignCatalogToCompany(companyModel.ParentId.Value, companyModel.Id,
                    companyModel.CatalogId.Value);
            }
        }

        private static CompanyModel Map(IUpdateCompanyCommand companyArguments)
        {
            return new CompanyModel
            {
                Id = companyArguments.CompanyId,
                BrandColorPrimary = companyArguments.BrandColorPrimary,
                BrandColorSecondary = companyArguments.BrandColorSecondary,
                BrandColorText = companyArguments.BrandColorText,
                City = companyArguments.City,
                Country = companyArguments.Country,
                Email = companyArguments.Email,
                Name = companyArguments.Name,
                PhoneNumber = companyArguments.PhoneNumber,
                State = companyArguments.State,
                StreetAddress = companyArguments.StreetAddress,
                SupportSiteUrl = companyArguments.SupportSiteUrl,
                ControlPanelSiteUrl = companyArguments.ControlPanelSiteUrl,
                Website = companyArguments.Website,
                ZipCode = companyArguments.ZipCode,
                LogoUrl = companyArguments.Logo,
                Domains = companyArguments.Domains,
                ParentId = companyArguments.ParentId,
                CatalogId = companyArguments.CatalogId
            };
        }
    }
}