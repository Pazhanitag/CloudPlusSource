using System.Linq;
using CloudPlus.Api.ViewModels.Request.Company;
using CloudPlus.Api.ViewModels.Response.Company;
using CloudPlus.Api.ViewModels.Response.Company.Branding;
using CloudPlus.Models.Company;
using CloudPlus.Models.Enums;
using System.Collections.Generic;

namespace CloudPlus.Api.Mappers
{
    public static class CompanyMapper
    {
        public static BrandingViewModel ToBrandingViewModel(this CompanyModel company, string urlContent)
        {
            if (company == null)
                return null;

            return new BrandingViewModel
            {
                PrimaryColor = company.BrandColorPrimary,
                SecondaryColor = company.BrandColorSecondary,
                TextColor = company.BrandColorText,
                LogoUrl = !string.IsNullOrWhiteSpace(company.LogoUrl) ? $"{urlContent}Static/Images/CompanyLogo/{company.LogoUrl}" : ""
            };
        }

        public static CompanyViewModel ToCompanyViewModel(this CompanyModel company, string urlContent)
        {
            if (company == null)
                return null;

            return new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                CompanyOu = company.CompanyOu,
                ParentId = company.ParentId,
                Status = company.Status,
                Type = company.Type,
                UniqueIdentifier = company.UniqueIdentifier,
                StreetAddress = company.StreetAddress,
                BrandColorPrimary = company.BrandColorPrimary,
                Email = company.Email,
                City = company.City,
                Country = company.Country,
                State = company.State,
                ZipCode = company.ZipCode,
                PhoneNumber = company.PhoneNumber,
                LogoUrl = !string.IsNullOrWhiteSpace(company.LogoUrl)
                    ? $"{urlContent}Static/Images/CompanyLogo/{company.LogoUrl}"
                    : "",
                LogoBase64 = string.Empty,
                BrandColorSecondary = company.BrandColorSecondary,
                BrandColorText = company.BrandColorText,
                Website = company.Website,
                SupportSite = company.SupportSiteUrl,
                ControlPanelSiteUrl = company.ControlPanelSiteUrl,
                CreateDate = company.CreateDate,
                Domains = company.Domains.Select(d => d.ToDomainViewModel()),
                WebsiteSameAsPrimaryDomain = false,
                NumberOfResellers = company.NumberOfResellers,
                NumberOfCustomers = company.NumberOfCustomers,
                NumberOfTotalCustomers = company.numberOfTotalCustomers,

                NumberOfTotalResellers = company.numberOfTotalResellers,
                NumberOfUsers = company.NumberOfUsers,
                CatalogId = company.CatalogId
            };
        }

        public static CompanyModel ToCompanyModel(this CreateCompanyViewModel company)
        {
            if (company == null)
                return null;

            return new CompanyModel
            {
                Name = company.Name,
                ParentId = company.ParentId,
                Status = CompanyStatus.Active,
                Type = (CompanyType)company.Type,
                StreetAddress = company.StreetAddress,
                City = company.City,
                State = company.State,
                ZipCode = company.ZipCode,
                PhoneNumber = company.PhoneNumber,
                Email = company.Email,
                BrandColorPrimary = company.BrandColorPrimary,
                BrandColorSecondary = company.BrandColorSecondary,
                BrandColorText = company.BrandColorText,
                CatalogId = company.CatalogId,
                LogoUrl = company.Logo,
                Website = company.Website,
                Country = company.Country,
                Domains = company.Domains.Select(d => d.ToDomainModel()),
                SupportSiteUrl = company.SupportSiteUrl,
                ControlPanelSiteUrl = company.ControlPanelSiteUrl,
                SendWelcomeLetters = company.User.SendWelcomeLetters
            };
        }

        public static dynamic ToCreateCompanyCommand(this CreateCompanyViewModel company)
        {
            if (company == null)
                return null;

            return new
            {
                Company = company.ToCompanyModel(),
                company.User.Email,
                company.User.UserName,
                company.User.FirstName,
                company.User.LastName,
                company.User.AlternativeEmail,
                company.User.JobTitle,
                company.User.DisplayName,
                company.User.PhoneNumber,
                company.User.StreetAddress,
                company.User.City,
                company.User.ZipCode,
                company.User.State,
                company.User.Country,
                company.User.CountryCode,
                company.User.Roles,
                company.User.PasswordSetupMethod,
                company.User.PasswordSetupEmail,
                company.User.SendPlainPasswordViaEmail,
                company.User.Password,
                company.User.ProfilePicture,
                CompanyDomain = company.User.Domain,
                company.User.CompanyName,
                company.User.UserStatus,
                company.User.SendWelcomeLetters,
                company.ClientDbId
            };
        }

        public static dynamic ToUpdateCompanyCommand(this UpdateCompanyViewModel company)
        {
            if (company == null)
                return null;

            return new
            {
                CompanyId = company.Id,
                company.ParentId,
                company.Name, 
                company.Website,
                company.LogoBase64,
                company.Logo,
                company.SupportSiteUrl,
                company.ControlPanelSiteUrl,
                company.Email,
                company.PhoneNumber,
                company.StreetAddress,
                company.City,
                company.ZipCode,
                company.State,
                company.Country,
                company.BrandColorPrimary,
                company.BrandColorSecondary,
                company.BrandColorText,
                company.Domains,
                company.CatalogId
            };
        }

        public static List<CompanyCountViewModel> ToCompanyCountViewModel(this CompanyCountModel model)
        {
            if (model == null)
                return null;

            var companyCounting = new List<CompanyCountViewModel>();

            foreach(var company in model.Children)
            {
                companyCounting.Add(new CompanyCountViewModel
                {
                    Id = company.Id,
                    NumberOfCustomers = company.NumberOfCustomers,
                    NumberOfResellers = company.NumberOfResellers,
                    NumberOfUsers = company.NumberOfUsers,
                    Type = ((int)company.Type).ToString()
                });
            }

            return companyCounting;
        }

        public static dynamic ToCompanyPriceCatalogViewModel(this CompanyModel company, string urlContent)
        {
            if (company == null)
                return null;

            return new
            {
                company.Id,
                company.Name,
                company.Type,
                Selected = false
            };
        }
    }
}
