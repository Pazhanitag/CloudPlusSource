using System;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.AppServices.Notification.Settings;
using CloudPlus.Enums.Notification;
using CloudPlus.Logging;
using CloudPlus.Models.Notification;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.EmailNotifications;
using CloudPlus.Services.Identity.User;
using CloudPlus.Settings;
using CloudPlus.Resources;
using System.Data;
using CloudPlus.Services.Database.Catalog;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using CloudPlus.Models.Catalog;
using CloudPlus.Services.Database.Support;

namespace CloudPlus.AppServices.Notification.Services
{
    public class EmailMessageService : IEmailMessageService
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly INotificationServiceSettings _notificationServiceSettings;
        private readonly IConfigurationManager _configurationManager;
        private readonly ICompanyCatalogService _companyCatalogService;
        private readonly ICustomSecureControlPanelService _customSecureControlPanelService;

        public EmailMessageService(
            IEmailTemplateService emailTemplateService,
            IUserService userService,
            ICompanyService companyService,
            INotificationServiceSettings notificationServiceSettings,
            IConfigurationManager configurationManager,
            ICompanyCatalogService companyCatalogService,
            ICustomSecureControlPanelService customSecureControlPanelService)
        {
            _emailTemplateService = emailTemplateService;
            _userService = userService;
            _companyService = companyService;
            _notificationServiceSettings = notificationServiceSettings;
            _configurationManager = configurationManager;
            _companyCatalogService = companyCatalogService;
            _customSecureControlPanelService = customSecureControlPanelService;
        }

        // TODO Write Unit Tests
        public EmailTemplateContentModel GetEmailTemplate(EmailTemplatePlaceholdersGeneratorRequestModel data)
        {
            try
            {
                var dictionary = GetEmailTemplatePlaceholderDictionary(data);

                if (data.EmailTemplateType == EmailTemplateType.Office365TransitionReport)
                    dictionary = GetTransitionEmailTemplatePlaceholderDictionary(data, dictionary);

                var requestData = new EmailTemplatePlaceholdersGeneratorModel
                {
                    EmailTemplateType = data.EmailTemplateType,
                    PlacehoderList = dictionary
                };

                var template = _emailTemplateService.GetEmailTemplate(requestData);

                return template;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetEmailTemplate!", ex);
                throw;
            }
        }

        // TODO Write Unit Tests
        private Dictionary<string, string> GetEmailTemplatePlaceholderDictionary(EmailTemplatePlaceholdersGeneratorRequestModel data)
        {
            try
            {
                var user = data.UserName != null ? _userService.GetUser(data.UserName) : null;

                var companyId = user?.CompanyId ?? data.CompanyId;

                var CustomSecurePanel = data.CustomSecurePanelId != 0 ? _customSecureControlPanelService.GetCustomSecurePanel(data.CustomSecurePanelId) : null;

                var company = _companyService.GetCompany(companyId);
                if (company == null)
                    throw new Exception($"No Comapny with id {companyId}");
                var parentCompanyId = company.ParentId ?? throw new Exception($"Comapny {company.Name} has no Parent Company Id");

                var parentCompany = _companyService.GetCompany(parentCompanyId);

                var companySupportSiteUrl = company.SupportSiteUrl == "" ? parentCompany.SupportSiteUrl : company.SupportSiteUrl;

                if (parentCompany == null)
                    throw new Exception($"No Parent Comapny with id {parentCompanyId}");

                var dictionary = new Dictionary<string, string>
                {
                    {EmailTemplatePlaceholdersConstants.UserDisplayName, user?.DisplayName ?? ""},
                    {EmailTemplatePlaceholdersConstants.UserLogin, user?.Email ?? ""},
                    {EmailTemplatePlaceholdersConstants.UserPasswordTempLink, data.TempResetLink},
                    {EmailTemplatePlaceholdersConstants.Password, data.Password},
                    {EmailTemplatePlaceholdersConstants.ARecordIp, _notificationServiceSettings.ARecordIp},
                    {EmailTemplatePlaceholdersConstants.CompanyName, company.Name ?? ""},
                    {
                        EmailTemplatePlaceholdersConstants.CompanyPrimaryDomain,
                        company.Domains.FirstOrDefault(d => d.IsPrimary)?.Name ?? ""
                    },
                    {EmailTemplatePlaceholdersConstants.CompanyControlPanelUrl, company.ControlPanelSiteUrl ?? ""},
                    {EmailTemplatePlaceholdersConstants.CompanySupportUrl, companySupportSiteUrl ?? ""},
                    {EmailTemplatePlaceholdersConstants.CompanyContactPhone, company.PhoneNumber ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyName, parentCompany.Name ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyWebSite, parentCompany.Website ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyControlPanelUrl, parentCompany.ControlPanelSiteUrl ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanySupportUrl, parentCompany.SupportSiteUrl ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyContactPhone, parentCompany.PhoneNumber ?? ""},
                    {EmailTemplatePlaceholdersConstants.ParentCompanyLogo, _configurationManager.GetByKey("ImageServerLogoPath")+"/CompanyLogo/"+parentCompany.LogoUrl},
                    {EmailTemplatePlaceholdersConstants.Domain, data.Domain},
                    {EmailTemplatePlaceholdersConstants.TxtRecord, data.TxtRecord},

                     {EmailTemplatePlaceholdersConstants.ControlPanelAddressStreet, CustomSecurePanel.CompanyAddressStreet ?? ""},
                     {EmailTemplatePlaceholdersConstants.ControlPanelAddressCity, CustomSecurePanel.CompanyAddressCity ?? ""},
                     {EmailTemplatePlaceholdersConstants.ControlPanelAddressState, CustomSecurePanel.CompanyAddressState ?? ""},
                     {EmailTemplatePlaceholdersConstants.ControlPanelAddressZip, CustomSecurePanel.CompanyAddressZipCode ?? ""},
                     {EmailTemplatePlaceholdersConstants.ControlPanelCompanyId, Convert.ToString(data.CompanyId) ?? ""},
                     {EmailTemplatePlaceholdersConstants.ControlPanelContactPhone, CustomSecurePanel.ContactPhone?? ""},
                     {EmailTemplatePlaceholdersConstants.ControlPanelContactName, CustomSecurePanel.ContactPerson?? ""},
                     {EmailTemplatePlaceholdersConstants.CustomSecureControlPanelURL, CustomSecurePanel.CustomSecureControlPanelURL?? ""},
                };

                return dictionary;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetEmailTemplatePlaceholderDictionary!", ex);
                throw;
            }
        }

        private Dictionary<string, string> GetTransitionEmailTemplatePlaceholderDictionary(EmailTemplatePlaceholdersGeneratorRequestModel data, Dictionary<string, string> dictionary)
        {
            try
            {
                dictionary.Add(EmailTemplatePlaceholdersConstants.TransitionDeletedUsersCount, data.Report.DeletedUsersSucceed.Count.ToString());
                dictionary.Add(EmailTemplatePlaceholdersConstants.TransitionAdminUsersCount, data.Report.AdminUsersSucceed.Count.ToString());
                dictionary.Add(EmailTemplatePlaceholdersConstants.TransitionLicensesUsersCount, data.Report.LicenseUsersSucceed.Count.ToString());

                var discussLicensesUsersTemp = "";
                string discussLicensesUsers;

                foreach (var user in data.Report.KeepLicensesUsers)
                    discussLicensesUsersTemp += "<li>" + user + "</li>";

                if (data.Report.KeepLicensesUsers.Count > 0)
                    discussLicensesUsers = "<ul>" + discussLicensesUsersTemp + "</ul>";
                else
                    discussLicensesUsers = "<ul><li>No users for discussing licenses!</li></ul>";

                dictionary.Add(EmailTemplatePlaceholdersConstants.TransitionDiscussLicensesUsers, discussLicensesUsers);

                var deleteUsersFailed = "";
                foreach (var user in data.Report.DeletedUsersFailed)
                    deleteUsersFailed += "<li>" + user + "</li>";

                var adminUsersFailed = "";
                foreach (var user in data.Report.AdminUsersSucceed)
                    adminUsersFailed += "<li>" + user + "</li>";

                var licensesUsersFailed = "";
                foreach (var user in data.Report.LicenseUsersSucceed)
                    licensesUsersFailed += "<li>" + user + "</li>";

                var failedUsers = "<ul>";
                if (data.Report.DeletedUsersFailed.Count > 0)
                    failedUsers += "<li>Delete users failed:<ul>" + deleteUsersFailed + "</ul></li>";

                if (data.Report.AdminUsersSucceed.Count > 0)
                    failedUsers += "<li>Admin users failed:<ul>" + adminUsersFailed + "</ul></li>";

                if (data.Report.LicenseUsersSucceed.Count > 0)
                    failedUsers += "<li>Licenses users failed:<ul>" + licensesUsersFailed + "</ul></li>";

                failedUsers = "</ul>";

                dictionary.Add(EmailTemplatePlaceholdersConstants.TransitionFailedUsers, failedUsers);

                return dictionary;
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured on GetTransitionEmailTemplatePlaceholderDictionary!", ex);
                throw;
            }
        }

        public async Task<Attachment> SendProductDetailsAsEmail(int companyId, int catalogId)
        {
            DataTable dtAllCatalog = new DataTable();
            dtAllCatalog = await _companyCatalogService.SendProductDetailsAsEmail(companyId, catalogId);
            System.IO.MemoryStream ms = DataTableToExcelXlsx(dtAllCatalog, "Sheet1");
            ms.Position = 0;
            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(ms, new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            attach.ContentDisposition.FileName = "Product_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            return attach;
        }

        public static MemoryStream DataTableToExcelXlsx(System.Data.DataTable table, string sheetName)
        {
            MemoryStream ReturnStream = new MemoryStream();
            try
            {
                ExcelPackage pack = new ExcelPackage();
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);

                int col = 1;
                int row = 1;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    ws.Cells[row, col].Value = table.Columns[i].ColumnName.ToString();
                    col++;
                }
                col = 1;
                row++;
                foreach (DataRow rw in table.Rows)
                {
                    foreach (DataColumn cl in table.Columns)
                    {
                        if (rw[cl.ColumnName] != DBNull.Value)
                            ws.Cells[row, col].Value = rw[cl.ColumnName].ToString();
                        col++;
                    }
                    row++;
                    col = 1;
                }
                pack.SaveAs(ReturnStream);

            }
            catch (Exception ex)
            {

            }
            return ReturnStream;
        }
    }
}
