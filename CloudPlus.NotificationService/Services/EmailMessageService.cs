using System;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.Api.NotificationService.Settings;
using CloudPlus.Enums.Notification;
using CloudPlus.Logging;
using CloudPlus.Models.Notification;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.EmailNotifications;
using CloudPlus.Services.Identity.User;
using CloudPlus.Settings;

namespace CloudPlus.Api.NotificationService.Services
{
    public class EmailMessageService : IEmailMessageService
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly INotificationServiceSettings _notificationServiceSettings;

        public EmailMessageService(
            IEmailTemplateService emailTemplateService,
            IUserService userService,
            ICompanyService companyService,
            INotificationServiceSettings notificationServiceSettings)
        {
            _emailTemplateService = emailTemplateService;
            _userService = userService;
            _companyService = companyService;
            _notificationServiceSettings = notificationServiceSettings;
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetWelcomeUserPasswordViaEmailTemplate(EmailTemplateType type,
            string userName, string tempResetLink)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, "", tempResetLink);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetWelcomeUserPasswordViaEmailTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetWelcomeUserSendPlainPasswordViaEmailTemplate(EmailTemplateType type,
            string userName, string password)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, password);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetWelcomeUserSendPlainPasswordViaEmailTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetWelcomeUserDontSendPasswordTemplate(EmailTemplateType type,
            string userName)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetWelcomeUserDontSendPasswordTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetForgotPasswordTemplate(EmailTemplateType type, string userName,
            string tempResetLink)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, "", tempResetLink);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetForgotPasswordTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetWelcomeCompanyCustomerTemplate(EmailTemplateType type,
            string userName, string tempResetLink, int companyId)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, "", tempResetLink);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetWelcomeCompanyCustomerTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetWelcomeCompanyResellerTemplate(EmailTemplateType type,
            string userName, string tempResetLink, int companyId)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, "", tempResetLink);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetWelcomeCompanyResellerTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetChangePasswordTemplate(EmailTemplateType type,
            string userName, string tempResetLink)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, "", tempResetLink);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetChangePasswordTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetPasswordChangedTemplate(EmailTemplateType type,
            string userName)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetPasswordChangedTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetPasswordChangedSendPlainPasswordViaEmailTemplate(EmailTemplateType type,
            string userName, string password)
        {
            try
            {
                var dictionary = GetWelcomeUserPlaceholderDictionary(userName, password);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = type,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetPasswordChangedSendPlainPasswordViaEmailTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        private Dictionary<string, string> GetWelcomeUserPlaceholderDictionary(string userName, string password = "",
            string tempResetLink = "", int companyId = 0)
        {
            try
            {
                var user = _userService.GetUser(userName);

                companyId = user?.CompanyId ?? throw new Exception($"No User with email {userName}");

                var company = _companyService.GetCompanyAsync(companyId).Result;
                if (company == null)
                    throw new Exception($"No Comapny with id {companyId}");

                var parentCompanyId = company.ParentId ?? throw new Exception($"Comapny {company.Name} has no Parent Company Id");

                var parentCompany = _companyService.GetCompanyAsync(parentCompanyId).Result;
                if (parentCompany == null)
                    throw new Exception($"No Parent Comapny with id {parentCompanyId}");

                var dictionary = new Dictionary<string, string>
                {
                    {EmailTemplatePlaceholdersConstants.UserDisplayName, user.DisplayName ?? ""},
                    {EmailTemplatePlaceholdersConstants.UserLogin, user.Email ?? ""},
                    {EmailTemplatePlaceholdersConstants.UserPasswordTempLink, tempResetLink},
                    {EmailTemplatePlaceholdersConstants.Password, password},
                    {EmailTemplatePlaceholdersConstants.ARecordIp, _notificationServiceSettings.ARecordIp},
                    {EmailTemplatePlaceholdersConstants.CompanyName, company.Name ?? ""},
                    {EmailTemplatePlaceholdersConstants.CompanyId, company.Id.ToString() ?? ""},
                    {
                        EmailTemplatePlaceholdersConstants.CompanyPrimaryDomain,
                        company.Domains.FirstOrDefault(d => d.IsPrimary)?.Name ?? ""
                    },
                    {EmailTemplatePlaceholdersConstants.CompanyControlPanelUrl, ""},
                    {EmailTemplatePlaceholdersConstants.CompanySupportUrl, ""},
                    {EmailTemplatePlaceholdersConstants.CompanyContactPhone, company.PhoneNumber ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyName, parentCompany.Name ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyWebSite, parentCompany.Website ?? ""},
                    // TODO Add this properties to database
                    {EmailTemplatePlaceholdersConstants.ParentCompanyControlPanelUrl,""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanySupportUrl, ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyContactPhone, parentCompany.PhoneNumber ?? ""},
                    // TODO Check is parentCompany.LogoUrl valid logo link
                    {EmailTemplatePlaceholdersConstants.ParentCompanyLogo, parentCompany.LogoUrl ?? ""}
                };

                return dictionary;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetPlaceholderDictionary!", ex);
                throw;
            }
        }
    }
}
