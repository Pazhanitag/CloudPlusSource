using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Office365;
using CloudPlus.Models.Office365.License;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Services.Database.Office365.User;

namespace CloudPlus.Services.Database.Office365.License
{
    public class Office365DbLicenseService : IOffice365DbLicenseService
    {
        private readonly CldpDbContext _cldpDbContext;
        private readonly IOffice365DbUserService _office365DbUserService;

        public Office365DbLicenseService(
            CldpDbContext cldpDbContext,
            IOffice365DbUserService office365DbUserService)
        {
            _cldpDbContext = cldpDbContext;
            _office365DbUserService = office365DbUserService;
        }

        public async Task<List<Office365OfferModel>> GetUserAssgnedLicenseAsync(string userPrincipalName)
        {
            var license = await _cldpDbContext.Office365Licenses
                .Include(i => i.Office365Offer).Where(l => l.Office365User.UserPrincipalName == userPrincipalName)
                .Select(x=>new Office365OfferModel() { CloudPlusProductIdentifier=x.Office365Offer.CloudPlusProductIdentifier,
                                                         OfferName=x.Office365Offer.Office365OfferName, IsAddon= x.Office365Offer.IsAddon

                }).ToListAsync();
            // .FirstOrDefaultAsync(l => l.Office365User.UserPrincipalName == userPrincipalName);
           
            return license;

            //return new Office365LicenseModel
            //{
            //    Id = license.Id,
            //    Office365Offer = new Office365OfferModel
            //    {
            //        Id = license.Office365Offer.Id,
            //        Office365Id = license.Office365Offer.Office365OfferId,
            //        OfferName = license.Office365Offer.Office365OfferName,
            //        Sku = license.Office365Offer.Office365ProductSku,
            //        CloudPlusProductIdentifier = license.Office365Offer.CloudPlusProductIdentifier
            //    }
            //};
        }

        public async Task CreateOffice365UserLicense(string userPrincipalName, string cloudPlusProductIdentifier)
        {
            var office365User = _cldpDbContext.Office365Users.FirstOrDefault(u => u.UserPrincipalName == userPrincipalName);
            var office365Offer = _cldpDbContext.Office365Offers.FirstOrDefault(o => o.CloudPlusProductIdentifier == cloudPlusProductIdentifier);

            var office365UserLicense = new Office365License
            {
                Office365User = office365User ?? throw new ArgumentException($"No Office 365 user with User Principal Name: {userPrincipalName}"),
                Office365Offer = office365Offer ?? throw new ArgumentException($"No Office 365 offer with CloudPlusProductIdentifier: {cloudPlusProductIdentifier}"),
            };

            _cldpDbContext.Office365Licenses.Add(office365UserLicense);

            await _cldpDbContext.SaveChangesAsync();
        }

        public async Task<Office365LicenseModel> RemoveOffice365UserLicense(string userPrincipalName, string cloudPlusProductIdentifier)
        {
            var office365User =
                await _office365DbUserService.GetOffice365DatabaseUserWithLicensesAndOfferAsync(userPrincipalName);

            var office365License = office365User.Licenses.FirstOrDefault(l =>
                l.Office365Offer.CloudPlusProductIdentifier == cloudPlusProductIdentifier);

            if (office365License == null)
                throw new ArgumentException($"Could not find user {userPrincipalName} " +
                    $"license with CloudPlus product identifier {cloudPlusProductIdentifier}");

            var licenseToRemove =
                await _cldpDbContext.Office365Licenses.FirstOrDefaultAsync(l => l.Id == office365License.Id);

            _cldpDbContext.Office365Licenses.Remove(licenseToRemove);

            await _cldpDbContext.SaveChangesAsync();

            return new Office365LicenseModel
            {
                Id = office365License.Id,
                Office365Offer = new Office365OfferModel
                {
                    Id = office365License.Office365Offer.Id,
                    Office365Id = office365License.Office365Offer.Office365Id,
                    OfferName = office365License.Office365Offer.OfferName,
                    Sku = office365License.Office365Offer.Sku,
                    CloudPlusProductIdentifier = office365License.Office365Offer.CloudPlusProductIdentifier
                }
            };
        }
    }
}
