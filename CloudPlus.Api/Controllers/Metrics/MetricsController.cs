using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Company;
using CloudPlus.Api.ViewModels.Response.Paging;
using CloudPlus.Api.ViewModels.Response.User;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Logging;
using CloudPlus.Models.Enums;
using CloudPlus.Models.Metrics;
using CloudPlus.QueueModels.Companies.Commands;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.Metrics;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Identity.Client;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Api.Controllers.Metrics
{
   // [Authorize]
    [RoutePrefix("api/metrics")]
    public class MetricsController : ApiController
    {
        #region Declaration

        private readonly IMetricsService _metricsService;

        #endregion Declaration

        #region Constructor

        public MetricsController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        #endregion Constructor

        #region Dashboard

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{userId:int}/{companyId:int}/{ReportPeriod:int}/Dashboard", Name = "Dashboard")]
        public IHttpActionResult Dashboard(int userId, int companyId, int ReportPeriod)
        {
            var result = _metricsService.Dashboard(userId,companyId, ReportPeriod);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{userId:int}/{companyId:int}/{ReportPeriod:int}/DashboardForScheduler", Name = "DashboardForScheduler")]
        public IHttpActionResult DashboardForScheduler(int userId, int companyId, int ReportPeriod)
        {
            var result = _metricsService.Dashboard(userId, companyId, ReportPeriod);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


        #endregion Dashboard

        #region Details Page

        #region Email Activity

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/EmailActivity", Name = "EmailActivity")]
        public IHttpActionResult GetEmailActivity(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetEmailActivity(companyId, ReportPeriod);
            var details = _metricsService.GetEmailActivityDetails(companyId, ReportPeriod,null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/EmailActivityDetails", Name = "EmailActivityDetails")]
        public IHttpActionResult EmailActivityDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetEmailActivityDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date}/EmailActivityDetails", Name = "EmailActivityDetailsByDate")]
        public IHttpActionResult EmailActivityDetails(int companyId, int ReportPeriod, string date)
        {
            var result = _metricsService.GetEmailActivityDetails(companyId, ReportPeriod,Convert.ToDateTime(date));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        

        #endregion Email Activity

        #region Active Users

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/ActiveUsers", Name = "ActiveUsers")]
        public IHttpActionResult GetActiveUsers(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetActiveUsers(companyId, ReportPeriod);
            var details = _metricsService.GetActiveUsersDetails(companyId, ReportPeriod, null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/ActiveUsersDetails", Name = "ActiveUsersDetails")]
        public IHttpActionResult GetActiveUsersDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetActiveUsersDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date:DateTime}/ActiveUsersDetails", Name = "ActiveUsersDetailsByDate")]
        public IHttpActionResult GetActiveUsersDetails(int companyId, int ReportPeriod, DateTime date)
        {
            var result = _metricsService.GetActiveUsersDetails(companyId, ReportPeriod, date);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Active Users

        #region OneDrive Storage

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/OneDriveFiles", Name = "OneDriveFiles")]
        public IHttpActionResult GetOneDriveStorageNew(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetOneDriveStorage(companyId, ReportPeriod);
            var details = _metricsService.GetOneDriveStorageDetails(companyId, ReportPeriod, null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/OneDriveFilesDetails", Name = "OneDriveFilesDetails")]
        public IHttpActionResult GetOneDriveStorageDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetOneDriveStorageDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date:DateTime}/OneDriveFilesDetails", Name = "OneDriveFilesDetailsByDate")]
        public IHttpActionResult GetOneDriveStorageDetails(int companyId, int ReportPeriod, DateTime date)
        {
            var result = _metricsService.GetOneDriveStorageDetails(companyId, ReportPeriod, date);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion OneDrive Storage

        #region SharePoint Activity

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/SharePointFiles", Name = "SharePointFiles")]
        public IHttpActionResult GetSharePointActivityCount(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetSharePointActivity(companyId, ReportPeriod);
            var details = _metricsService.GetSharePointActivityDetails(companyId, ReportPeriod, null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/SharePointFilesDetails", Name = "SharePointFilesDetails")]
        public IHttpActionResult GetSharePointActivityDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetSharePointActivityDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date:DateTime}/SharePointFilesDetails", Name = "SharePointFilesDetailsByDate")]
        public IHttpActionResult GetSharePointActivityDetails(int companyId, int ReportPeriod, DateTime date)
        {
            var result = _metricsService.GetSharePointActivityDetails(companyId, ReportPeriod, date);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion SharePoint Activity

        #region Skype For Business Activity

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/SkypeforBusinessActivity", Name = "SkypeforBusinessActivity")]
        public IHttpActionResult GetSkypeForBusinessActivityCount(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetSkypeForBusinessActivity(companyId, ReportPeriod);
            var details = _metricsService.GetSkypeForBusinessActivityDetails(companyId, ReportPeriod, null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/SkypeforBusinessActivityDetails", Name = "SkypeforBusinessActivityDetailsByDate")]
        public IHttpActionResult GetSkypeForBusinessActivityDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetSkypeForBusinessActivityDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date:DateTime?}/SkypeforBusinessActivityDetails", Name = "SkypeforBusinessActivityDetails")]
        public IHttpActionResult GetSkypeForBusinessActivityDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            var result = _metricsService.GetSkypeForBusinessActivityDetails(companyId, ReportPeriod, date);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Skype For Business Activity

        #region Office 365 Activations

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/OfficeActivations", Name = "OfficeActivations")]
        public IHttpActionResult GetOfficeActivationCount(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetOfficeActivation(companyId, ReportPeriod);
            var details = _metricsService.GetOfficeActivationDetails(companyId, ReportPeriod, null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/OfficeActivationsDetails", Name = "OfficeActivationsDetails")]
        public IHttpActionResult GetOfficeActivationDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetOfficeActivationDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date:DateTime}/OfficeActivationsDetails", Name = "OfficeActivationsDetailsByDate")]
        public IHttpActionResult GetOfficeActivationDetails(int companyId, int ReportPeriod, DateTime date)
        {
            var result = _metricsService.GetOfficeActivationDetails(companyId, ReportPeriod, date);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Office 365 Activations

        #region Teams Activity

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/MicrosoftTeamsActivity", Name = "MicrosoftTeamsActivity")]
        public IHttpActionResult GetTeamsActivityCount(int companyId, int ReportPeriod)
        {
            var count = _metricsService.GetTeamsActivity(companyId, ReportPeriod);
            var details = _metricsService.GetTeamsActivityDetails(companyId, ReportPeriod, null);
            var result = new
            {
                count,
                details
            };
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/MicrosoftTeamsActivityDetails", Name = "MicrosoftTeamsActivityDetails")]
        public IHttpActionResult GetTeamsActivityDetails(int companyId, int ReportPeriod)
        {
            var result = _metricsService.GetTeamsActivityDetails(companyId, ReportPeriod, null);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{companyId:int}/{ReportPeriod:int}/{date:DateTime}/MicrosoftTeamsActivityDetails", Name = "MicrosoftTeamsActivityDetailsByDate")]
        public IHttpActionResult GetTeamsActivityDetails(int companyId, int ReportPeriod, DateTime date)
        {
            var result = _metricsService.GetTeamsActivityDetails(companyId, ReportPeriod, date);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Teams Activity

        #endregion Details Page

        #region Vendor Metrics Admin and Customer Config / Customization

        #region Get all  Vendor Metrics 

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("AllWidgets", Name = "AllWidgets")]
        public IHttpActionResult GetAllWidgets()
        {
            var result = _metricsService.GetAllWidgets();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Get all  Vendor Metrics 

        #region Save widgets for all companies

        [HttpPost]
        [Route("UpdateVendorMetrics", Name = "UpdateVendorMetrics")]
        public IHttpActionResult UpdateVendorMetrics(List<VendorMetricsModel> configuration)
        {
            // VendorMetricsAdminConfigModel configuration = new VendorMetricsConfigurationModel();
            var result = _metricsService.SaveWidgetsForAllCompanies(configuration);
            if (result == null)
                return NotFound();
            return Ok(configuration);
        }

        [HttpPost]
        [Route("ResetAllCompanies", Name = "ResetAllCompanies")]
        public IHttpActionResult ResetAllCompanies()
        {
            // VendorMetricsAdminConfigModel configuration = new VendorMetricsConfigurationModel();
            var result = _metricsService.ResetAllCompanies();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Save widgets for all companies

        #region Vendor Metrics Customer Configuration
        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{CompanyId:int}/{CustomerId:int}/CustomerWidgets", Name = "CustomerWidgets")]
        public IHttpActionResult CustomerWidgets(int CompanyId,int CustomerId)
        {
            var result = _metricsService.GetVendorMetricsUserConfigByUserId(CompanyId, CustomerId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        //[ValidateModel]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("UpdateCustomerWidgets",Name = "UpdateCustomerWidgets")]        
        public IHttpActionResult UpdateCustomerWidgets(List<VendorMetricsUserConfigModel> configuration)
        {
            // VendorMetricsAdminConfigModel configuration = new VendorMetricsConfigurationModel();
            var result = _metricsService.UpdateVendorMetricsUserConfig(configuration);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


        #endregion Venodr Metrics Customer Configuration

        #region Vendor Metrics Admin Config

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{CompanyId:int}/CompanyWidgets", Name = "CompanyWidgets")]
        public IHttpActionResult CompanyWidgets(int CompanyId)
        {
            var result = _metricsService.GetVendorMetricsAdminConfigByCompanyId(CompanyId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        //[ValidateModel]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("UpdateCompanyWidgets", Name = "UpdateCompanyWidgets")]
        public IHttpActionResult UpdateCompanyWidgets(List<VendorMetricsAdminConfigModel> configuration)
        {
            var result = _metricsService.UpdateVendorMetricsAdminConfig(configuration);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        #endregion Vendor Metrics Admin Config

        #endregion Vendor Metrics Admin and User Config / Customization

        #region Vendor  Metrics Report config
        [HttpPost]
        //[ValidateModel]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("SaveVendorMetricsReportConfigs", Name = "SaveVendorMetricsReportConfigs")]
        public IHttpActionResult SaveVendorMetricsReportConfigs(VendorMetricsReportConfigsModel _VendorMetricsReportConfigsModel)
        {  
            var result = _metricsService.SaveVendorMetricsReportConfigs(_VendorMetricsReportConfigsModel);
            if (result == null)
                return NotFound();
            return Ok(true);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{Id:int}/GetReportConfigBasedOnId", Name = "GetReportConfigBasedOnId")]
        public IHttpActionResult GetReportConfigBasedOnId(int Id)
        {
            var result = _metricsService.GetReportConfigBasedOnId(Id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
        [Route("{UserId:int}/GetReportConfig", Name = "GetReportConfig")]
        public IHttpActionResult GetReportConfig(int userId)
        {
            var result = _metricsService.GetReportConfig(userId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        #endregion Vendor  Metrics Report config
    }
}
