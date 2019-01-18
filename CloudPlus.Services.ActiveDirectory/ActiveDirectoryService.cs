using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Resources;
using CloudPlus.Services.ActiveDirectory.Models;
using CloudPlus.Logging;

namespace CloudPlus.Services.ActiveDirectory
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly HttpClient _httpClient;

        public ActiveDirectoryService(IConfigurationManager configurationManager,
            IHttpClientResolver httpClientResolver)
        {
            _configurationManager = configurationManager;
            _httpClient = httpClientResolver.GetActiveDirectoryHttpClient();
        }

	    public async Task<bool> UserExists(ActiveDirectoryUser user)
	    {
		    if (user == null)
			    throw new ArgumentNullException(nameof(user));

		    var response = await _httpClient.GetAsync($"{_configurationManager.GetByKey("AD.User")}?upn={user.Upn}");

		    if (response.IsSuccessStatusCode)
		    {
				return true;
		    }

		    return false;
	    }

		public async Task CreateUser(ActiveDirectoryUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

			var response = await _httpClient.PostAsJsonAsync(_configurationManager.GetByKey("AD.User"), user);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to create AD user {user.Upn}");
            }
        }

        public async Task Update(ActiveDirectoryUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var response = await _httpClient.PutAsJsonAsync(_configurationManager.GetByKey("AD.User"), user);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to update AD user {user.Upn}");
            }
        }

        public async Task DeleteUser(string upn)
        {
            if (string.IsNullOrWhiteSpace(upn))
                throw new ArgumentException($"{nameof(upn)} cannot be null");

            var response = await _httpClient.DeleteAsync(string.Format(_configurationManager.GetByKey("AD.UserDelete"), upn));

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to delete AD user {upn}");
        }

        public async Task UpdateUserPassword(string upn, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(upn) || string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Upn and password cannot be null or whitespace");

            var response = await _httpClient.PutAsJsonAsync(_configurationManager.GetByKey("AD.UserPassword"), new
            {
                Upn = upn,
                NewPassword = newPassword
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to update password for a user {upn}");

        }

        public async Task CreateCompany(ActiveDirectoryCompany company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            var response = await _httpClient.PostAsJsonAsync(_configurationManager.GetByKey("AD.Company"), company);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to create AD company with CompanyOu {company.CompanyOu}");
            }
        }

        public async Task DeleteCompany(int companyOu)
        {
            this.Log().Info($"Start deleting Company on Active Directory with CompanyOu {companyOu}");
            var response =
                await _httpClient.DeleteAsync(string.Format(_configurationManager.GetByKey("AD.CompanyDelete"), companyOu));

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to delete Company from Active Directory with CompanyOu: {companyOu}");
        }

        public async Task<int> GenerateCompanyOuIdAsync()
        {
            var response = await _httpClient.GetAsync(_configurationManager.GetByKey("AD.GenerateOuId"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to generate new OU Id");

            var ouId = await response.Content.ReadAsAsync<HttpResponse>();
        
            return Convert.ToInt32(ouId.Result);
        }
    }
}
