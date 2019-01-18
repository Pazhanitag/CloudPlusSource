using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Entities;
using CloudPlus.Models.Enums;
using CloudPlus.Models.Metrics;

namespace CloudPlus.Services.Database.Metrics
{
    public interface IMetricsService
    {
        #region Dashboard

        /// <summary>
        /// Method to return the information for the dashboard chart
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DashboardModel</returns>
        VendorMetrics_DashboardModel Dashboard(int userId, int companyId, int ReportPeriod);

        #endregion Dashboard

        #region Vendor Metrics configuration

        #region Admin Config

        /// <summary>
        /// Retrieve vendor metrics Admin configuartions based on the CompanyId
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>IEnumerable<VendorMetricsAdminConfigModel></returns>
        IEnumerable<VendorMetricsAdminConfigModel> GetVendorMetricsAdminConfigByCompanyId(int CompanyId);

        /// <summary>
        /// Update the configuration of a company by the Admin
        /// </summary>
        /// <param name="CompanyConfigurations"></param>
        /// <returns>IEnumerable<VendorMetricsAdminConfigModel></returns>
        IEnumerable<VendorMetricsAdminConfigModel> UpdateVendorMetricsAdminConfig(List<VendorMetricsAdminConfigModel> configList);
        #endregion Admin Config

        #region Customer Config

        /// <summary>
        /// Get the metric list of the user based on the metrics configured for the company
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>IEnumerable<VendorMetricsUserConfigModel></returns>
        IEnumerable<VendorMetricsUserConfigModel> GetVendorMetricsUserConfigByUserId(int CompanyId, int UserId);

        /// <summary>
        /// Update the configuration of a Users to customize the dashboard by the user itself
        /// </summary>
        /// <param name="CompanyConfigurations"></param>
        /// <returns>IEnumerable<VendorMetricsUserConfigModel></returns>
        IEnumerable<VendorMetricsUserConfigModel> UpdateVendorMetricsUserConfig(List<VendorMetricsUserConfigModel> configList);
        #endregion Customer Config

        #region Get All Metrics
        /// <summary>
        /// Get list of widgets available
        /// </summary>
        /// <returns> IEnumerable<VendorMetricsModel></returns>
        IEnumerable<VendorMetricsModel> GetAllWidgets();

        #endregion Get All Metrics

        #region SaveWidgetsForAllCompanies

        /// <summary>
        /// Common method to save the widgets for all the companies
        /// </summary>
        /// <param name="vendorMetrics"></param>
        /// <returns></returns>
        bool SaveWidgetsForAllCompanies(List<VendorMetricsModel> vendorMetrics);

        /// <summary>
        /// Reset values for all the companies based on the Admin config
        /// </summary>
        /// <returns></returns>
        bool ResetAllCompanies();

        /// <summary>
        /// Reset values for a specific company based on the Admin config
        /// </summary>
        /// <returns></returns>
        bool SetCompanyMetrics(int CompanyId);

        #endregion SaveWidgetsForAllCompanies

        #endregion Vendor Metrics configuration

        #region Details Page

        #region Details Chart

        /// <summary>
        /// Retrieve the Email Activity
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>   
        VendorMetrics_DetailsPageChartModel GetEmailActivity(int companyId, int ReportPeriod);

        /// <summary>
        /// Retrieve the Active Users Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>  
        VendorMetrics_DetailsPageChartModel GetActiveUsers(int companyId, int ReportPeriod);

        /// <summary>
        /// Retrieve the OneDriveStorage
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns> 
        VendorMetrics_DetailsPageChartModel GetOneDriveStorage(int companyId, int ReportPeriod);

        /// <summary>
        /// Retrieve the Office 365 Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>
        VendorMetrics_DetailsPageChartModel GetOfficeActivation(int companyId, int ReportPeriod);

        /// <summary>
        /// Retrieve the Sharepoint Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>
        VendorMetrics_DetailsPageChartModel GetSharePointActivity(int companyId, int ReportPeriod);

        /// <summary>
        /// Retrieve the skype for business Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>
        VendorMetrics_DetailsPageChartModel GetSkypeForBusinessActivity(int companyId, int ReportPeriod);

        /// <summary>
        /// Retrieve the Teams Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>
        VendorMetrics_DetailsPageChartModel GetTeamsActivity(int companyId, int ReportPeriod);

        #endregion Details Chart

        #region Details Grid

        /// <summary>
        /// Retrieve the Email Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getEmailActivityUserDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getEmailActivityUserDetailModel> GetEmailActivityDetails(int companyId, int ReportPeriod, DateTime? date);

        /// <summary>
        /// Retrieve the Email Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getOffice365ActiveUserDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getOffice365ActiveUserDetailModel> GetActiveUsersDetails(int companyId, int ReportPeriod, DateTime? date);

        /// <summary>
        /// Retrieve the office 365 Activation summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getOffice365ActivationsUserDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getOffice365ActivationsUserDetailModel> GetOfficeActivationDetails(int companyId, int ReportPeriod, DateTime? date);

        /// <summary>
        /// Retrieve the onedrive storage summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getOneDriveUsageAccountDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getOneDriveUsageAccountDetailModel> GetOneDriveStorageDetails(int companyId, int ReportPeriod, DateTime? date);

        /// <summary>
        /// Retrieve the SharePoint Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getSharePointActivityUserDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getSharePointActivityUserDetailModel> GetSharePointActivityDetails(int companyId, int ReportPeriod, DateTime? date);

        /// <summary>
        /// Retrieve the skypefor business  Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getSkypeForBusinessActivityUserDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getSkypeForBusinessActivityUserDetailModel> GetSkypeForBusinessActivityDetails(int companyId, int ReportPeriod, DateTime? date);

        /// <summary>
        /// Retrieve theTeams  Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getTeamsUserActivityUserDetailModel></returns>
        IEnumerable<VendorMetrics_Office365_getTeamsUserActivityUserDetailModel> GetTeamsActivityDetails(int companyId, int ReportPeriod, DateTime? date);

        bool SaveVendorMetricsReportConfigs(VendorMetricsReportConfigsModel _VendorMetricsReportConfigsModel);

        IEnumerable<VendorMetricsReportConfigsModel> GetReportConfigBasedOnId(int Id);

        IEnumerable<VendorMetricsReportConfigsModel> GetReportConfig(int userId);
        #endregion Details Grid

        #endregion Details Page
    }

}


