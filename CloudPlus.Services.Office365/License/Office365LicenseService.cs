using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Store.PartnerCenter.Models.Licenses;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.License;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Resources;
using CloudPlus.Services.Office365.Operations;

namespace CloudPlus.Services.Office365.License
{
    public class Office365LicenseService : IOffice365LicenseService
    {
        private readonly IPartnerOperations _partnerOperations;
        private readonly int _retryAttempts;

        public Office365LicenseService(IPartnerOperations partnerOperations, IConfigurationManager configurationManager)
        {
            _partnerOperations = partnerOperations;
            _retryAttempts = int.Parse(configurationManager.GetByKey("RetryAttempts"));
        }

        public async Task<List<Office365LicenseModel>> GetAllUserLicenses(string office365CustomerId, string office365UserId)
        {
            var licenses = await _partnerOperations.UserPartnerOperations.Customers
                .ById(office365CustomerId).Users.ById(office365UserId).Licenses.GetAsync();

            return licenses.Items.Select(l => new Office365LicenseModel
            {
                Office365Offer = new Office365OfferModel
                {
                    OfferName = l.ProductSku.Name,
                    Sku = l.ProductSku.Id
                }
            }).ToList();
        }

        public async Task AssignUserLicense(string office365CustomerId, string office365UserId, string office365ProductSku)
        {
            var license = new LicenseAssignment
            {
                SkuId = office365ProductSku,
                ExcludedPlans = null
            };

            var licenseList = new List<LicenseAssignment> { license };
            var updateLicense = new LicenseUpdate { LicensesToAssign = licenseList };

            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    // Assign licenses to the user on Partner Partal
                    await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Users.ById(office365UserId).LicenseUpdates.CreateAsync(updateLicense);

                    await ConfirmAssignUserLicense(office365CustomerId, office365UserId, office365ProductSku);

                    requestSuccess = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Assign license to user request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            if (!requestSuccess) throw new Exception("Could not assign User License!");
        }

        public async Task RemoveUserLicense(string office365CustomerId, string office365UserId, string office365ProductSku)
        {
            var licenseToRemove = new List<string> { office365ProductSku };

            var updateLicense = new LicenseUpdate { LicensesToRemove = licenseToRemove };

            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    // Remove licenses from the user on Partner Partal
                    await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Users.ById(office365UserId).LicenseUpdates.CreateAsync(updateLicense);

                    requestSuccess = true;

                    await ConfirmRemoveUserLicens(office365CustomerId, office365UserId, office365ProductSku);
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Remove license from user request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            if (!requestSuccess) throw new Exception("Could not remove User License!");
        }

        public async Task RemoveUserMultiLicenses(string office365CustomerId, string office365UserId, List<string> office365ProductSkuList)
        {
            var updateLicense = new LicenseUpdate { LicensesToRemove = office365ProductSkuList };

            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    // Remove licenses from the user on Partner Partal
                    await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Users.ById(office365UserId).LicenseUpdates.CreateAsync(updateLicense);

                    requestSuccess = true;

                    await ConfirmRemoveUserLicens(office365CustomerId, office365UserId, office365ProductSkuList.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Remove license from user request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            if (!requestSuccess) throw new Exception("Could not remove User Licenses!");
        }

        private async Task ConfirmAssignUserLicense(string office365CustomerId, string office365UserId, string office365ProductSku)
        {
            var licenseAssigned = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the license with Sku {office365ProductSku} to be assigned. Take {attempts}");

                    var assignedLicenses = await _partnerOperations.UserPartnerOperations.Customers
                        .ById(office365CustomerId).Users.ById(office365UserId).Licenses.GetAsync();

                    if (assignedLicenses.Items.All(i => !string.Equals(i.ProductSku.Id, office365ProductSku, StringComparison.InvariantCultureIgnoreCase)))
                        throw new NullReferenceException($"License with Sku {office365ProductSku} is not assigned yet");

                    licenseAssigned = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!licenseAssigned && attempts < _retryAttempts);

            if (!licenseAssigned) throw new Exception("Could not confirm User License assignment!");
        }

        private async Task ConfirmRemoveUserLicens(string office365CustomerId, string office365UserId, string office365ProductSku)
        {
            var licenseRemoved = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the license with Sku {office365ProductSku} to be removed. Take {attempts}");

                    var assignedLicenses = await _partnerOperations.UserPartnerOperations.Customers
                        .ById(office365CustomerId).Users.ById(office365UserId).Licenses.GetAsync();

                    if (assignedLicenses.Items.Any(i => string.Equals(i.ProductSku.Id, office365ProductSku, StringComparison.InvariantCultureIgnoreCase)))
                        throw new NullReferenceException($"License with Sku {office365ProductSku} is not removed yet");

                    licenseRemoved = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!licenseRemoved && attempts < _retryAttempts);

            if (!licenseRemoved) throw new Exception("Could not confirm User License removing!");
        }
    }
}
