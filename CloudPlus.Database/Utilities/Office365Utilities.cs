using System;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.Entities.Office365;

namespace CloudPlus.Database.Utilities
{
    public class Office365Utilities
    {
        private readonly CldpDbContext _dbContext;

        public Office365Utilities(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedOffice365Roles()
        {
            var exchangeServiceAdministrator = new Office365Role
            {
                Name = "Exchange Service Administrator",
                Office365Id = "29232cdf-9323-42fd-ade2-1d097af3e4de",
                Description = "Exchange Service Administrator."
            };

            var lyncServiceAdministrator = new Office365Role
            {
                Name = "Lync Service Administrator",
                Office365Id = "75941009-915a-4869-abe7-691bff18279e",
                Description = "Lync Service Administrator."
            };

            var serviceSupportAdministrator = new Office365Role
            {
                Name = "Service Support Administrator",
                Office365Id = "f023fd81-a637-4b56-95fd-791ac0226033",
                Description = "Service Support Administrator has access to perform common support tasks."
            };

            var sharepointServiceAdministrator = new Office365Role
            {
                Description = "SharePoint Service Administrator.",
                Name = "SharePoint Service Administrator",
                Office365Id = "f28a1f50-f6e7-4571-818b-6a12f2af6b6c"
            };

            var companyAdministrator = new Office365Role
            {
                Description = "Company Administrator role has full access to perform any operation in the company scope.",
                Name = "Company Administrator",
                Office365Id = "62e90394-69f5-4237-9190-012177145e10"
            };

            var billingAdministrator = new Office365Role
            {
                Description = "Billing Administrator has access to perform common billing related tasks.",
                Name = "Billing Administrator",
                Office365Id = "b0f54661-2d74-4c50-afa3-1ec803f12efe"
            };

            var userAccountAdministrator = new Office365Role
            {
                Description = "User Account Administrator has access to perform common user management related tasks.",
                Name = "User Account Administrator",
                Office365Id = "fe930be7-5e62-47db-91af-98c3a49a38b1"
            };

            _dbContext.Office365Roles.AddRange(new List<Office365Role>
            {
                exchangeServiceAdministrator,
                lyncServiceAdministrator,
                serviceSupportAdministrator,
                sharepointServiceAdministrator,
                companyAdministrator,
                billingAdministrator,
                userAccountAdministrator
            });

            _dbContext.SaveChanges();
        }

        public void SeedOffice365Offers()
        {
            var office365BusinessEssentials = new Office365Offer
            {
                Office365OfferId = "BD938F12-058F-4927-BBA3-AE36B1D2501C",
                Office365OfferName = "Office 365 Business Essentials",
                Office365ProductSku = "3B555118-DA6A-4418-894F-7DF1E2096870",
                CloudPlusProductIdentifier = "3B555118-DA6A-4418-894F-7DF1E2096870"
            };

            var office365Business = new Office365Offer
            {
                Office365OfferId = "5C9FD4CC-EDCE-44A8-8E91-07DF09744609",
                Office365OfferName = "Office 365 Business",
                Office365ProductSku = "CDD28E44-67E3-425E-BE4C-737FAB2899D3",
                CloudPlusProductIdentifier = "CDD28E44-67E3-425E-BE4C-737FAB2899D3"
            };

            var office365BusinessPremium = new Office365Offer
            {
                Office365OfferId = "031C9E47-4802-4248-838E-778FB1D2CC05",
                Office365OfferName = "Office 365 Business Premium",
                Office365ProductSku = "F245ECC8-75AF-4F8E-B61F-27D8114DE5F3",
                CloudPlusProductIdentifier = "F245ECC8-75AF-4F8E-B61F-27D8114DE5F3"
            };

            var office365ProPlus = new Office365Offer
            {
                Office365OfferId = "BE57FF4C-100C-4F1F-B82D-F1C5AB63A665",
                Office365OfferName = "Office 365 ProPlus",
                Office365ProductSku = "C2273BD0-DFF7-4215-9EF5-2C7BCFB06425",
                CloudPlusProductIdentifier = "C2273BD0-DFF7-4215-9EF5-2C7BCFB06425"
            };

            var office365EnterpriseE1 = new Office365Offer
            {
                Office365OfferId = "91FD106F-4B2C-4938-95AC-F54F74E9A239",
                Office365OfferName = "Office 365 Enterprise E1",
                Office365ProductSku = "18181A46-0D4E-45CD-891E-60AABD171B4E",
                CloudPlusProductIdentifier = "18181A46-0D4E-45CD-891E-60AABD171B4E"
            };

            var office365EnterpriseE3 = new Office365Offer
            {
                Office365OfferId = "796B6B5F-613C-4E24-A17C-EBA730D49C02",
                Office365OfferName = "Office 365 Enterprise E3",
                Office365ProductSku = "6FD2C87F-B296-42F0-B197-1E91E994B900",
                CloudPlusProductIdentifier = "6FD2C87F-B296-42F0-B197-1E91E994B900"
            };

            var office365EnterpriseE5WithoutAudioConferencing = new Office365Offer
            {
                Office365OfferId = "4F7ECAF1-E9D6-4CAC-9687-E22EB3DFDD70",
                Office365OfferName = "Office 365 Enterprise E5 without Audio Conferencing",
                Office365ProductSku = "26D45BD9-ADF1-46CD-A9E1-51E9A5524128",
                CloudPlusProductIdentifier = "26D45BD9-ADF1-46CD-A9E1-51E9A5524128"
            };

            var exchangeOnlinePlan1 = new Office365Offer
            {
                Office365OfferId = "195416C1-3447-423A-B37B-EE59A99A19C4",
                Office365OfferName = "Exchange Online (Plan 1)",
                Office365ProductSku = "4B9405B0-7788-4568-ADD1-99614E613B69",
                CloudPlusProductIdentifier = "4B9405B0-7788-4568-ADD1-99614E613B69"
            };

            var exchangeOnlinePlan2 = new Office365Offer
            {
                Office365OfferId = "2F707C7C-2433-49A5-A437-9CA7CF40D3EB",
                Office365OfferName = "Exchange Online (Plan 2)",
                Office365ProductSku = "19EC0D23-8335-4CBD-94AC-6050E30712FA",
                CloudPlusProductIdentifier = "19EC0D23-8335-4CBD-94AC-6050E30712FA"
            };

            var exchangeOnlineKiosk = new Office365Offer
            {
                Office365OfferId = "35A36B80-270A-44BF-9290-00545D350866",
                Office365OfferName = "Exchange Online Kiosk",
                Office365ProductSku = "80B2D799-D2BA-4D2A-8842-FB0D0F3A4B82",
                CloudPlusProductIdentifier = "80B2D799-D2BA-4D2A-8842-FB0D0F3A4B82"
            };

            var exchangeOnlineProtection = new Office365Offer
            {
                Office365OfferId = "D903A2DB-BF6F-4434-83F1-21BA44017813",
                Office365OfferName = "Exchange Online Protection",
                Office365ProductSku = "45A2423B-E884-448D-A831-D9E139C52D2F",
                CloudPlusProductIdentifier = "45A2423B-E884-448D-A831-D9E139C52D2F"
            };

            var exchangeThreatProtection = new Office365Offer
            {
                Office365OfferId = "A2706F86-868D-4048-989B-0C69E5C76B63",
                Office365OfferName = "Exchange Online Advanced Threat Protection",
                Office365ProductSku = "4EF96642-F096-40DE-A3E9-D83FB2F90211",
                CloudPlusProductIdentifier = "4EF96642-F096-40DE-A3E9-D83FB2F90211"
            };

            var exchangeOnlineArchivingForExchangeOnline = new Office365Offer
            {
                Office365OfferId = "2828BE95-46BA-4F91-B2FD-0BEF192ECF60",
                Office365OfferName = "Exchange Online Archiving for Exchange Online",
                Office365ProductSku = "EE02FD1B-340E-4A4B-B355-4A514E4C8943",
                CloudPlusProductIdentifier = "EE02FD1B-340E-4A4B-B355-4A514E4C8943"
            };


            _dbContext.Office365Offers.AddRange(new List<Office365Offer>
            {
                office365BusinessEssentials,
                office365Business,
                office365BusinessPremium,
                office365ProPlus,
                office365EnterpriseE1,
                office365EnterpriseE3,
                office365EnterpriseE5WithoutAudioConferencing,
                exchangeOnlinePlan1,
                exchangeOnlinePlan2,
                exchangeOnlineKiosk,
                exchangeOnlineProtection,
                exchangeOnlineArchivingForExchangeOnline
            });

            _dbContext.SaveChanges();
        }

        public void SeedOffice365RolesDisplayNamesAndOrd()
        {
            var dbOffice365Roles = _dbContext.Office365Roles.ToList();

            var order = 0;

            dbOffice365Roles.ForEach(role =>
            {
                order += 100;
                role.DisplayName = role.Name;
                role.Ord = order;
            });

            var companyAdministrator = dbOffice365Roles.FirstOrDefault(r => r.Name == "Company Administrator");
            companyAdministrator.DisplayName = "Company Administrator (Global Admin)";
            companyAdministrator.Ord = Int32.MaxValue;

            _dbContext.SaveChanges();
        }
    }
}
