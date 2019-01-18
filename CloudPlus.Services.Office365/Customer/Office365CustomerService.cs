using System;
using System.Threading.Tasks;
using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Customer;
using CloudPlus.Resources;
using CloudPlus.Services.Office365.Operations;
using CloudPlus.Services.Office365.Utilities;

// ReSharper disable once CheckNamespace
namespace CloudPlus.Services.Office365.CustomerService
{
    public class Office365CustomerService : IOffice365CustomerService
    {
        private readonly IPartnerOperations _partnerOperations;
        private readonly IOffice365UtilitiesService _office365Utilities;
        private readonly int _retryAttempts;

        public Office365CustomerService(
            IPartnerOperations partnerOperations,
            IOffice365UtilitiesService office365Utilities,
            IConfigurationManager configurationManager)
        {
            _partnerOperations = partnerOperations;
            _office365Utilities = office365Utilities;
            _retryAttempts = int.Parse(configurationManager.GetByKey("RetryAttempts"));
        }

        public async Task<Office365PartnerPlatformCustomerModel> CreatePartnerPlatformCustomerAsync(Office365PartnerPlatformCustomerModel customer)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var defaultMicrosoftDomain = await _office365Utilities.GenerateDefaultMicrosoftDomainAsync(customer.CompanyOu);
                    this.Log().Info($"Finished generating default Microsoft domain: {defaultMicrosoftDomain}");

                    var newCustomer =
                        await _partnerOperations.UserPartnerOperations.Customers.CreateAsync(
                            new Customer
                            {
                                CompanyProfile = new CustomerCompanyProfile
                                {
                                    Domain = defaultMicrosoftDomain,
                                    CompanyName = customer.CompanyName
                                },
                                BillingProfile = new CustomerBillingProfile
                                {
                                    CompanyName = customer.CompanyName,
                                    Culture = customer.Culture,
                                    Email = customer.Email,
                                    FirstName = customer.FirstName,
                                    LastName = customer.LastName,
                                    Language = customer.Language,
                                    DefaultAddress = new Address
                                    {
                                        City = customer.City,
                                        State = customer.State,
                                        AddressLine1 = customer.AddressLine1,
                                        AddressLine2 = customer.AddressLine2,
                                        PostalCode = customer.PostalCode,
                                        Country = customer.Country,
                                        PhoneNumber = customer.PhoneNumber,
                                        FirstName = customer.FirstName,
                                        LastName = customer.LastName
                                    }
                                }
                            });

                    requestSuccess = true;

                    await ConfirmCreated(newCustomer.Id);

                    customer.Id = newCustomer.Id;

                    return customer;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Create partner platform customer request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Create partner platform customer Failed!");
        }

        public async Task<string> GetCustomerDefaultDomainAsync(string office365CustomerId)
        {
            var customer = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                .GetAsync();

            return customer.CompanyProfile.Domain;
        }

        private async Task ConfirmCreated(string newCustomerId)
        {
            var customerCreated = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the customer {newCustomerId} to be created. Take {attempts}");

                    await _partnerOperations.UserPartnerOperations.Customers.ById(newCustomerId).GetAsync();

                    customerCreated = true;
                }
                catch (Exception)
                {
                    this.Log().Error($"Failed getting customer {newCustomerId}");
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!customerCreated && attempts < _retryAttempts);

            if (!customerCreated) throw new Exception("Could not confirm Customer creation!");
        }
    }
}
