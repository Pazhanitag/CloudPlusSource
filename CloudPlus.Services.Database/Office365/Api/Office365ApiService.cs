using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Models.Office365.Transition;
using CloudPlus.Models.Office365.User;
using CloudPlus.Resources;

namespace CloudPlus.Services.Database.Office365.Api
{
    public class Office365ApiService : IOffice365ApiService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly HttpClient _httpClient;
        private readonly int _retryAttempts;

        public Office365ApiService(
            IConfigurationManager configurationManager,
            IHttpClientResolver httpClientResolver)
        {
            _configurationManager = configurationManager;
            _httpClient = httpClientResolver.GetOffice365ApiHttpClient();
            _retryAttempts = int.Parse(configurationManager.GetByKey("RetryAttempts"));
        }

        public async Task AddCustomerDomainAsync(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.AddCustomerDomain"), model);

            var domainAdded = await response.Content.ReadAsAsync<HttpResponse>();

            if (!Convert.ToBoolean(domainAdded.Result))
                throw new Exception($"Unable to add customer domain. Office365CustomerId: {model.Office365CustomerId}, Domain: {model.Domain}");
        }

        public async Task<string> GetCustomerIdByDomainAsync(IOffice365CustomerDomainModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Domain))
                throw new ArgumentNullException(nameof(model));

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.GetCustomerIdByDomain"), model);

            var customerId = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToString(customerId.Result);
        }

        public async Task<string> GetCustomerDomainTxtRecordsAsync(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.GetDomainTxtRecord"), model);

            var txtRecords = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToString(txtRecords.Result);
        }

        public async Task RemoveCustomerDomainAsync(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.RemoveCustomerDomain"), model);

            var domainRemoved = await response.Content.ReadAsAsync<HttpResponse>();

            if (!Convert.ToBoolean(domainRemoved.Result))
                throw new Exception($"Unable to remove customer domain. Office365CustomerId: {model.Office365CustomerId}, Domain: {model.Domain}");
        }

        public async Task<bool> VerifyCustomerDomainAsync(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.VerifyCustomerDomain"), model);

            var domainVerified = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToBoolean(domainVerified.Result);
        }

        public async Task<bool> FederateCustomerDomainAsync(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.FederateCustomerDomain"), model);

            var domainFederated = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToBoolean(domainFederated.Result);
        }

        public async Task<string> CreateOffice365UserAsync(Office365ApiUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.Users"), model);

            var office365UserId = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToString(office365UserId.Result);
        }

        public async Task UserHardDeleteAsync(Office365ApiUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await TryExecutePost(_configurationManager.GetByKey("Office365Api.UserHardDelete"), model);
        }

        public async Task<List<string>> GetUserRoles(Office365UserRolesModel model)
        {
            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.GetUserRoles"), model);
            var roles = await response.Content.ReadAsAsync<HttpResponse>();

            return JsonConvert.DeserializeObject<List<string>>(roles.Result.ToString());
        }

        public async Task AssingUserRoles(Office365UserRolesModel model)
        {
            await TryExecutePost(_configurationManager.GetByKey("Office365Api.AssignUserRoles"), model);
        }

        public async Task RemoveUserRoles(Office365UserRolesModel model)
        {
            await TryExecutePost(_configurationManager.GetByKey("Office365Api.RemoveUserRoles"), model);
        }

        public async Task<bool> IsDomainVerified(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.IsDomainVerified"), model);

            var domainVerified = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToBoolean(domainVerified.Result);
        }

        public async Task<bool> IsDomainfederated(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.IsDomainFederated"), model);

            var domainVerified = await response.Content.ReadAsAsync<HttpResponse>();

            return Convert.ToBoolean(domainVerified.Result);
        }

        public async Task<List<Office365TransitionBasicMatchingDataModel>> GetTransitionMatchingDataAsync(IOffice365CustomerDomainModel model)
        {
            ValidateOffice365CustomerDomainModel(model);

            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.GetTransitionMatchingData"), model);

            var transitionMatchingData = await response.Content.ReadAsAsync<HttpResponse>();

            return JsonConvert.DeserializeObject<List<Office365TransitionBasicMatchingDataModel>>(transitionMatchingData.Result.ToString());
        }

        public async Task<bool> SetImmutableId(Office365ImmutableIdModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Office365CustomerId) || string.IsNullOrWhiteSpace(model.UserPrincipalName))
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var response = await TryExecutePost(_configurationManager.GetByKey("Office365Api.SetUserImmutableId"), model);

            if(!response.IsSuccessStatusCode)
                throw new Exception($"Unable to set immutable id fo user {model.UserPrincipalName}");
            
            var resut = await response.Content.ReadAsAsync<HttpResponse>();
            
            return Convert.ToBoolean(resut.Result);        
        }

        private void ValidateOffice365CustomerDomainModel(IOffice365CustomerDomainModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.Domain) || string.IsNullOrWhiteSpace(model.Office365CustomerId))
                throw new ArgumentNullException($"Domain and Office365CustomerId cannot be null: Domain {model.Domain}, Office365CustomerId: {model.Office365CustomerId}");
        }

        private async Task<HttpResponseMessage> TryExecutePost(string endpoint, object model)
        {
            var attemptsCount = 1;
            var response = await _httpClient.PostAsJsonAsync(endpoint, model);

            while (!response.IsSuccessStatusCode && attemptsCount < _retryAttempts)
            {
                await Task.Delay(3000);
                response = await _httpClient.PostAsJsonAsync(endpoint, model);
                attemptsCount++;
            }

            if (!response.IsSuccessStatusCode) throw new Exception("Error executing Powershell script!");

            return response;
        }
    }
}
