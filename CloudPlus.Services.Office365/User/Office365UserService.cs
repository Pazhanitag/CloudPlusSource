using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Store.PartnerCenter.Models.Query;
using Microsoft.Store.PartnerCenter.Models.Users;
using CloudPlus.Enums.Office365;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.User;
using CloudPlus.Resources;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Office365.Operations;

namespace CloudPlus.Services.Office365.User
{
    public class Office365UserService : IOffice365UserService
    {
        private readonly IPartnerOperations _partnerOperations;
        private readonly IOffice365ApiService _office365ApiService;
        private readonly int _retryAttempts;

        public Office365UserService(IPartnerOperations partnerOperations, IConfigurationManager configurationManager)
        {
            _partnerOperations = partnerOperations;
            _retryAttempts = int.Parse(configurationManager.GetByKey("RetryAttempts"));
        }

        public async Task<string> GetOffice365UserIdAsync(string userPrincipalName, string office365CustomerId)
        {
            var filter = new SimpleFieldFilter("UserPrincipalName",
                FieldFilterOperation.StartsWith, userPrincipalName);
            var qurey = QueryFactory.Instance.BuildSimpleQuery(filter);

            var queryResult = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId).Users
                .QueryAsync(qurey);

            if (queryResult.Items.Count() == 1) // We need to ensure that only one result is returned. Anything else and it is false positive
            {
                return queryResult.Items.FirstOrDefault()?.Id;
            }

            return null;
        }

        public async Task<List<Office365UserModel>> GetAllOffice365Users(string office365CustomerId)
        {
            var users = await _partnerOperations.UserPartnerOperations
                .Customers.ById(office365CustomerId).Users.GetAsync();

            return users.Items.Select(u => new Office365UserModel
            {
                Office365UserId = u.Id,
                UserPrincipalName = u.UserPrincipalName,
                Office365UserState = (Office365UserState)u.State,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName
            }).ToList();
        }

        public async Task<Office365SdkUser> CreateOffice365UserAsync(Office365SdkUser user)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var createdUser = await _partnerOperations.UserPartnerOperations.Customers.ById(user.Office365CustomerId).Users.CreateAsync(
                        new CustomerUser
                        {
                            PasswordProfile = new PasswordProfile
                            {
                                Password = user.Password,
                                ForceChangePassword = false
                            },
                            DisplayName = user.DisplayName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserPrincipalName = user.UserPrincipalName,
                            UsageLocation = user.UsageLocation
                        });

                    await ConfirmUserCreated(user.Office365CustomerId, createdUser.Id);

                    user.Office365UserId = createdUser.Id;

                    requestSuccess = true;

                    return user;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Create Office 365 User request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Create Office 365 User!");
        }

        public async Task DeleteOffice365UserAsync(string office365UserId, string office365CustomerId)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Users.ById(office365UserId).DeleteAsync();

                    await ConfirmUserDeleted(office365CustomerId, office365UserId);

                    requestSuccess = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Delete Office 365 User request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            if (!requestSuccess) throw new Exception("Could not delete Office 365 user!");
        }

        public async Task RestoreOffice365UserAsync(string office365UserId, string office365CustomerId)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var updatedCustomerUser = new CustomerUser
                    {
                        State = UserState.Active
                    };

                    await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Users.ById(office365UserId).PatchAsync(updatedCustomerUser);

                    await ConfirmUserRestore(office365CustomerId, office365UserId);

                    requestSuccess = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Create Office 365 User request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            if (!requestSuccess) throw new Exception("Could not restore Office 365 user!");
        }

        private async Task ConfirmUserCreated(string office365CustomerId, string office365UserId)
        {
            var userCreated = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the user with id {office365UserId} to be created. Take {attempts}");

                    await _partnerOperations.UserPartnerOperations
                        .Customers.ById(office365CustomerId).Users.ById(office365UserId).GetAsync();

                    userCreated = true;
                }
                catch (Exception)
                {
                    this.Log().Error($"User with id {office365UserId} is not created yet");
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!userCreated && attempts < _retryAttempts);

            if (!userCreated) throw new Exception("Could not confirm user creation!");
        }

        private async Task ConfirmUserDeleted(string office365CustomerId, string office365UserId)
        {
            var userDeleted = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the user with id {office365UserId} to be deleted. Take {attempts}");

                    await _partnerOperations.UserPartnerOperations
                        .Customers.ById(office365CustomerId).Users.ById(office365UserId).GetAsync();

                    this.Log().Error($"User with id {office365UserId} is not deleted yet");
                    attempts++;
                    await Task.Delay(3000);
                }
                catch (Exception)
                {
                    userDeleted = true;
                }
            } while (!userDeleted && attempts < _retryAttempts);

            if (!userDeleted) throw new Exception("Could not confirm user deletion!");
        }

        private async Task ConfirmUserRestore(string office365CustomerId, string office365UserId)
        {
            var userRestored = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the user with id {office365UserId} to be restored. Take {attempts}");

                    await _partnerOperations.UserPartnerOperations
                        .Customers.ById(office365CustomerId).Users.ById(office365UserId).GetAsync();

                    userRestored = true;
                }
                catch (Exception)
                {
                    this.Log().Error($"User with id {office365UserId} is not restored yet");
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!userRestored && attempts < _retryAttempts);

            if (!userRestored) throw new Exception("Could not confirm user restoration!");
        }
    }
}
