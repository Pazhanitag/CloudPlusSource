using System;
using System.Collections.Generic;
using CloudPlus.Database;
using System.Linq;
using CloudPlus.Models.Company;
using System.Data.Entity;
using System.Threading.Tasks;
using CloudPlus.Entities.Catalog;
using CloudPlus.Logging;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using CloudPlus.Services.Identity.User;
using CloudPlus.Models.Metrics;
using System.Data.Entity.Infrastructure;
using CloudPlus.Entities;

namespace CloudPlus.Services.Database.Metrics
{
    public class MetricsService : IMetricsService
    {
        #region Declaration

        private readonly CldpDbContext _dbContext;
        private readonly IUserService _userService;

        #endregion Declaration

        #region Constructor

        public MetricsService(
            CldpDbContext dbContext,
            IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        #endregion Constructor

        #region Dashboard

        /// <summary>
        /// Method to return the information for the dashboard chart
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns></returns>
        public VendorMetrics_DashboardModel Dashboard(int userId, int companyId, int ReportPeriod)
        {
            CheckandAddUserConfig(companyId, userId);
            var metricsList = from metrics in _dbContext.VendorMetrics
                              join companyConfig in _dbContext.VendorMetricsAdminConfig on metrics.Id equals companyConfig.MetricsId
                              join UserConfig in _dbContext.VendorMetricsUserConfig on metrics.Id equals UserConfig.MetricsId
                              where UserConfig.UserId == userId && UserConfig.CanAccess == true && metrics.CanAccess == true && companyConfig.CompanyId == companyId
                              orderby metrics.Id
                              select new
                              {
                                  metrics.Name,
                                  metrics.DashboardChartType,
                                  metrics.DetailsChartType
                              };

            OtherProperties otherProp;
            VendorMetrics_DashboardModel _dashboardModel = new VendorMetrics_DashboardModel();
            _dashboardModel.Metrics = new List<string>();
            _dashboardModel.ChartType = new List<int>();
            _dashboardModel.GraphData = new List<GraphData>();

            if (metricsList != null)
            {
                foreach (var metric in metricsList)
                {
                    if (metric != null)
                    {
                        switch (metric.Name)
                        {
                            case "Email Activity":
                                {
                                    // Retrive email activity details from the VendorMetrics_Office365_getEmailActivityCounts table
                                    //************************************************************************************************************************************************************************//
                                    var EmailActivityCount1 = _dbContext.VendorMetrics_Office365_getEmailActivityCounts
                                                                    .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                                                    .GroupBy(u => new { u.CompanyId, u.ReportPeriod })
                                                                    .Select(u => new
                                                                    {
                                                                        Send = u.Sum(x => x.Send),
                                                                        Receive = u.Sum(x => x.Receive),
                                                                        Read = u.Sum(x => x.Read)
                                                                    }).FirstOrDefault();

                                    //Assigning email activity details to the model which is to be returned
                                    if (EmailActivityCount1 != null)
                                    {
                                        int total = (Convert.ToInt32(EmailActivityCount1.Read) + Convert.ToInt32(EmailActivityCount1.Receive) + Convert.ToInt32(EmailActivityCount1.Send));
                                        decimal trend = Math.Round(GetEmailActivityTrend(companyId, ReportPeriod, (Convert.ToInt32(EmailActivityCount1.Read) + Convert.ToInt32(EmailActivityCount1.Receive) + Convert.ToInt32(EmailActivityCount1.Send))), 2);

                                        otherProp = new OtherProperties
                                        {
                                            Total = total,
                                            Trend = Convert.ToDouble(trend),
                                            Url = "EmailActivity",
                                            imageUrl = "/static/images/email.png",
                                            ToolTip = "Number of send / receive actions"
                                        };
                                        SetGraphValues(metric.Name, metric.DashboardChartType, EmailActivityCount1, otherProp, ref _dashboardModel);
                                    }
                                    //************************************************************************************************************************************************************************//
                                }
                                break;
                            case "Active Users":
                                //Retrive active users details from the VendorMetrics_Office365_getOffice365ActiveUserDetail table
                                //************************************************************************************************************************************************************************//
                                var ActiveUsers1 = _dbContext.VendorMetrics_Office365_getOffice365ActiveUserDetail
                                                                .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod && x.IsDeleted == false);

                                //Assigning active users details to the model which is to be returned
                                if (ActiveUsers1 != null && ActiveUsers1.Count() > 0)
                                {
                                    int Total = Convert.ToInt32(ActiveUsers1.Count());
                                    double trend = 0;
                                    var ActiveUsersMetrics = new ActiveUsers
                                    {
                                        Office365 = Convert.ToInt32(ActiveUsers1.Where(x => x.HasOneDriveLicense == true).Count()),
                                        Exchange = Convert.ToInt32(ActiveUsers1.Where(x => x.HasExchangeLicense == true).Count()),
                                        OneDrive = Convert.ToInt32(ActiveUsers1.Where(x => x.HasOneDriveLicense == true).Count()),
                                        SharePoint = Convert.ToInt32(ActiveUsers1.Where(x => x.HasSharePointLicense == true).Count()),
                                        SkypeForBusiness = Convert.ToInt32(ActiveUsers1.Where(x => x.HasSkypeForBusinessLicense == true).Count()),
                                        Yammer = Convert.ToInt32(ActiveUsers1.Where(x => x.HasYammerLicense == true).Count()),
                                        Teams = Convert.ToInt32(ActiveUsers1.Where(x => x.HasTeamsLicense == true).Count()),
                                    };
                                    otherProp = new OtherProperties
                                    {
                                        Total = Total,
                                        Trend = Convert.ToDouble(trend),
                                        Url = "ActiveUsers",
                                        imageUrl = "/static/images/active_users.svg",
                                        ToolTip = "MS Usage"
                                    };
                                    SetGraphValues(metric.Name, metric.DashboardChartType, ActiveUsersMetrics, otherProp, ref _dashboardModel);
                                    //************************************************************************************************************************************************************************//
                                }
                                break;
                            case "OneDrive Files":
                                //Retrive Onedrive usage count from the VendorMetrics_Office365_getOneDriveUsageStorage table
                                //************************************************************************************************************************************************************************//
                                var OneDriveUsage1 = _dbContext.VendorMetrics_Office365_getOneDriveUsageStorage
                                                            .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod && x.SiteType == "OneDrive")
                                                            .GroupBy(u => new { u.CompanyId, u.ReportPeriod, u.ReportDate })
                                                            .Select(u => new
                                                            {
                                                                u.FirstOrDefault().ReportDate,
                                                                StorageUsed = u.Sum(x => x.StorageUsed)
                                                            }
                                                            ).ToList();

                                //Assigning Onedrive usage count details to the model which is to be returned
                                if (OneDriveUsage1 != null && OneDriveUsage1.Count() > 0)
                                {
                                    List<OneDriveMetric> metricList = new List<OneDriveMetric>();
                                    double Total = 0;
                                    foreach (var usage in OneDriveUsage1)
                                    {
                                        metricList.Add(new OneDriveMetric
                                        {
                                            Date = usage.ReportDate.ToString("MM/dd/yyyy"),
                                            StorageUsed = FormatBytes(usage.StorageUsed)
                                        });
                                        Total += usage.StorageUsed;
                                    }
                                    Total = Convert.ToDouble(FormatBytes(Total));
                                    double trend = Math.Round(GetOneDriveTrend(companyId, ReportPeriod, Total), 2);
                                    var onedriveMetrics = new OneDriveUsage
                                    {
                                        Total = Total.ToString(),
                                        Trend = trend,
                                        Usages = metricList
                                    };

                                    otherProp = new OtherProperties
                                    {
                                        Total = Total,
                                        Trend = trend,
                                        Url = "OneDriveFiles",
                                        imageUrl = "/static/images/one_drive.png",
                                        ToolTip = "Latest number of files in OneDrive"
                                    };
                                    SetSpecialGraphValues(metric.Name, metric.DashboardChartType, metricList, otherProp, ref _dashboardModel);
                                }
                                //************************************************************************************************************************************************************************//
                                break;
                            case "SharePoint Files":
                                //Retrive SharePoint Activity count from the VendorMetrics_Office365_getSharePointActivityUserCounts table
                                //************************************************************************************************************************************************************************//
                                var SPActivityUserCount1 = _dbContext.VendorMetrics_Office365_getSharePointActivityUserCounts
                                                                .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                                                 .GroupBy(u => new { u.CompanyId, u.ReportPeriod })
                                                                .Select(u => new
                                                                {
                                                                    ViewedOrEdited = u.Sum(x => x.ViewedOrEdited),
                                                                    Synced = u.Sum(x => x.Synced),
                                                                    SharedExternally = u.Sum(x => x.SharedExternally),
                                                                    SharedInternally = u.Sum(x => x.SharedInternally)
                                                                }).FirstOrDefault();

                                //Assigning SharePoint Activity details to the model which is to be returned
                                if (SPActivityUserCount1 != null)
                                {
                                    int total = Convert.ToInt32(SPActivityUserCount1.ViewedOrEdited)
                                                   + Convert.ToInt32(SPActivityUserCount1.Synced)
                                                   + Convert.ToInt32(SPActivityUserCount1.SharedExternally)
                                                   + Convert.ToInt32(SPActivityUserCount1.SharedInternally);

                                    double trend = Math.Round((GetSharePointTrend(companyId, ReportPeriod, total)), 2);
                                    otherProp = new OtherProperties
                                    {
                                        Total = total,
                                        Trend = trend,
                                        Url = "SharePointFiles",
                                        imageUrl = "/static/images/share_point.png",
                                        ToolTip = "Latest number of files in SharePoint"
                                    };

                                    SetGraphValues(metric.Name, metric.DashboardChartType, SPActivityUserCount1, otherProp, ref _dashboardModel);
                                }
                                //************************************************************************************************************************************************************************//
                                break;
                            case "Skype for Business Activity":
                                //Retrive Skype Activity count from the VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts table
                                //************************************************************************************************************************************************************************//
                                var SkypeActivityUserCount1 = _dbContext.VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts
                                                                    .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                                                    .GroupBy(u => new { u.CompanyId, u.ReportPeriod })
                                                                    .Select(u => new
                                                                    {
                                                                        PeerToPeer = u.Sum(x => x.PeerToPeer),
                                                                        Organized = u.Sum(x => x.Organized),
                                                                        Participated = u.Sum(x => x.Participated)
                                                                    }).FirstOrDefault();

                                //Assigning Skype Activity details to the model which is to be returned
                                if (SkypeActivityUserCount1 != null)
                                {
                                    int total = Convert.ToInt32(SkypeActivityUserCount1.PeerToPeer)
                                                    + Convert.ToInt32(SkypeActivityUserCount1.Organized)
                                                    + Convert.ToInt32(SkypeActivityUserCount1.Participated);

                                    double trend = Math.Round(GetSkypeTrend(companyId, ReportPeriod, Convert.ToInt32(SkypeActivityUserCount1.PeerToPeer)
                                                    + Convert.ToInt32(SkypeActivityUserCount1.Organized)
                                                    + Convert.ToInt32(SkypeActivityUserCount1.Participated)), 2);

                                    _dashboardModel.skypeActivity = new SkypeActivity
                                    {
                                        PeerToPeer = Convert.ToInt32(SkypeActivityUserCount1.PeerToPeer),
                                        Organized = Convert.ToInt32(SkypeActivityUserCount1.Organized),
                                        Participated = Convert.ToInt32(SkypeActivityUserCount1.Participated),
                                        Total = total,
                                        Trend = trend
                                    };
                                    otherProp = new OtherProperties
                                    {
                                        Total = total,
                                        Trend = trend,
                                        Url = "SkypeforBusinessActivity",
                                        imageUrl = "/static/images/skype.png",
                                        ToolTip = "Total Skype activity"
                                    };
                                    SetGraphValues(metric.Name, metric.DashboardChartType, SkypeActivityUserCount1, otherProp, ref _dashboardModel);
                                }
                                //************************************************************************************************************************************************************************//
                                break;
                            case "Office Activations":
                                //Retrive ActivationCount from the VendorMetrics_Office365_getOffice365ActivationCounts table
                                var ActivationCount1 = _dbContext.VendorMetrics_Office365_getOffice365ActivationCounts.Where(x => x.CompanyId == companyId).FirstOrDefault();

                                //Assigning Activation Count details to the model which is to be returned
                                if (ActivationCount1 != null)
                                {
                                    int total = Convert.ToInt32(ActivationCount1.Windows)
                                                    + Convert.ToInt32(ActivationCount1.Android)
                                                    + Convert.ToInt32(ActivationCount1.Mac)
                                                    + Convert.ToInt32(ActivationCount1.Android)
                                                    + Convert.ToInt32(ActivationCount1.Windows10Mobile);
                                    var actCount = new ActivationCount
                                    {
                                        Desktop = Convert.ToInt32(ActivationCount1.Windows) + Convert.ToInt32(ActivationCount1.Mac),
                                        Devices = Convert.ToInt32(ActivationCount1.iOS)
                                                    + Convert.ToInt32(ActivationCount1.Android)
                                                    + Convert.ToInt32(ActivationCount1.Windows10Mobile)
                                    };

                                    _dashboardModel.activationCount = actCount;
                                    otherProp = new OtherProperties
                                    {
                                        Total = total,
                                        Url = "OfficeActivations",
                                        imageUrl = "/static/images/office.png",
                                        ToolTip = "Total number of Office activations"
                                    };
                                    SetGraphValues(metric.Name, metric.DashboardChartType, actCount, otherProp, ref _dashboardModel);
                                }
                                //************************************************************************************************************************************************************************//
                                break;
                            case "Microsoft Teams Activity":
                                //Retrive Teams Activity count from the VendorMetrics_Office365_getTeamsUserActivityUserCounts table
                                //************************************************************************************************************************************************************************//
                                var TeamsActivityUserCount1 = _dbContext.VendorMetrics_Office365_getTeamsUserActivityUserCounts
                                                                    .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                                                    .GroupBy(u => new { u.CompanyId, u.ReportPeriod })
                                                                    .Select(u => new
                                                                    {
                                                                        TeamChatMessages = u.Sum(x => x.TeamChatMessages),
                                                                        PrivateChatMessages = u.Sum(x => x.PrivateChatMessages),
                                                                        Calls = u.Sum(x => x.Calls),
                                                                        Meetings = u.Sum(x => x.Meetings),
                                                                        OtherActions = u.Sum(x => x.OtherActions)
                                                                    }).FirstOrDefault();

                                //Assigning Teams Activity details to the model which is to be returned
                                if (TeamsActivityUserCount1 != null)
                                {
                                    int total = Convert.ToInt32(TeamsActivityUserCount1.PrivateChatMessages)
                                                        + Convert.ToInt32(TeamsActivityUserCount1.TeamChatMessages)
                                                        + Convert.ToInt32(TeamsActivityUserCount1.Calls)
                                                        + Convert.ToInt32(TeamsActivityUserCount1.Meetings)
                                                        + Convert.ToInt32(TeamsActivityUserCount1.OtherActions);

                                    double trend = Math.Round((GetTeamsTrend(companyId, ReportPeriod, total)), 2);
                                    _dashboardModel.teamsActivity = new TeamsActivity
                                    {
                                        ChatMessages = Convert.ToInt32(TeamsActivityUserCount1.PrivateChatMessages) + Convert.ToInt32(TeamsActivityUserCount1.TeamChatMessages),
                                        ChannelMessages = Convert.ToInt32(TeamsActivityUserCount1.Calls) + Convert.ToInt32(TeamsActivityUserCount1.Meetings) + Convert.ToInt32(TeamsActivityUserCount1.OtherActions),
                                        Total = total,
                                        Trend = trend
                                    };
                                    otherProp = new OtherProperties
                                    {
                                        Total = total,
                                        Trend = trend,
                                        Url = "MicrosoftTeamsActivity",
                                        imageUrl = "/static/images/teams.png",
                                        ToolTip = "MS Teams"
                                    };
                                    SetGraphValues(metric.Name, metric.DashboardChartType, TeamsActivityUserCount1, otherProp, ref _dashboardModel);
                                }

                                //************************************************************************************************************************************************************************//
                                break;
                        }

                    }
                }
            }
            return _dashboardModel;
        }

        #region All Trends for Dashboard

        /// <summary>
        /// Get Email activity trend by summing the last week information 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        public decimal GetEmailActivityTrend(int companyId, int ReportPeriod, decimal Total)
        {
            decimal emailTrend = 0;
            int targetPeriod = 30;
            if (ReportPeriod != 7)
            {
                targetPeriod = ReportPeriod == 30 ? 90 : 180;
            }
            var OldMaxDate = _dbContext.VendorMetrics_Office365_getEmailActivityCounts
                                .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod).OrderBy(x => x.ReportDate).FirstOrDefault().ReportDate;

            DateTime startDate = OldMaxDate.Date.AddDays(-7);
            DateTime endDate = startDate.Date.AddDays(-7);


            decimal oldSum = 0;
            var result = _dbContext.VendorMetrics_Office365_getEmailActivityCounts
                                .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod && x.ReportDate < startDate.Date && x.ReportDate >= endDate)
                                .GroupBy(x => new { x.CompanyId, x.ReportPeriod })
                                 .Select(u => new
                                 {
                                     sum = u.Sum(x => x.Send) + u.Sum(x => x.Read) + u.Sum(x => x.Receive)
                                 }).FirstOrDefault();
            if (result != null)
                emailTrend = ((Total - Convert.ToDecimal(oldSum)) / Convert.ToDecimal(oldSum)) * 100;

            return decimal.Round(emailTrend, 2, MidpointRounding.AwayFromZero); ;
        }

        public decimal GetActiveUsersTrend(int companyId, int ReportPeriod, decimal Total)
        {
            decimal emailTrend = 0;
            int targetPeriod = 30;
            if (ReportPeriod != 7)
            {
                targetPeriod = ReportPeriod == 30 ? 90 : 180;
            }
            var OldMaxDate = _dbContext.VendorMetrics_Office365_getOffice365ActiveUserCounts
                                .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod).OrderBy(x => x.ReportDate).FirstOrDefault().ReportDate;

            DateTime startDate = OldMaxDate.Date.AddDays(-7);
            DateTime endDate = startDate.Date.AddDays(-7);


            decimal oldSum = 0;
            var result = _dbContext.VendorMetrics_Office365_getOffice365ActiveUserCounts
                                .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod && x.ReportDate < startDate.Date && x.ReportDate >= endDate)
                                .GroupBy(x => new { x.CompanyId, x.ReportPeriod })
                                 .Select(u => new
                                 {
                                     sum = u.Sum(x => x.Exchange) + u.Sum(x => x.Office365) + u.Sum(x => x.OneDrive) + u.Sum(x => x.SharePoint) + u.Sum(x => x.SkypeForBusiness) + u.Sum(x => x.Teams) + u.Sum(x => x.Yammer)
                                 }).FirstOrDefault();
            if (result != null)
                emailTrend = ((Total - Convert.ToDecimal(oldSum)) / Convert.ToDecimal(oldSum)) * 100;

            return decimal.Round(emailTrend, 2, MidpointRounding.AwayFromZero); ;
        }

        public double GetOneDriveTrend(int companyId, int ReportPeriod, double Total)
        {
            double onedriveTrend = 0;
            int targetPeriod = 30;
            if (ReportPeriod != 7)
            {
                targetPeriod = ReportPeriod == 30 ? 90 : 180;
            }
            var OldMaxDate = _dbContext.VendorMetrics_Office365_getOneDriveUsageStorage
                               .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod && x.SiteType == "OneDrive").OrderBy(x => x.ReportDate).FirstOrDefault().ReportDate;

            DateTime startDate = OldMaxDate.Date.AddDays(-7);
            DateTime endDate = startDate.Date.AddDays(-7);

            var oldSum = _dbContext.VendorMetrics_Office365_getOneDriveUsageStorage
                                .Where(x => x.CompanyId == companyId && x.ReportDate < startDate.Date && x.ReportDate >= endDate && x.SiteType == "OneDrive")
                                .GroupBy(x => new { x.CompanyId, x.ReportPeriod })
                                 .Select(u => new
                                 {
                                     sum = u.Sum(x => x.StorageUsed)
                                 }).FirstOrDefault();
            onedriveTrend = ((Total - Convert.ToDouble(oldSum.sum)) / Convert.ToDouble(oldSum.sum)) * 100;
            return onedriveTrend;
        }

        public double GetSharePointTrend(int companyId, int ReportPeriod, double Total)
        {
            double onedriveTrend = 0;
            int targetPeriod = 30;
            if (ReportPeriod != 7)
            {
                targetPeriod = ReportPeriod == 30 ? 90 : 180;
            }
            var OldMaxDate = _dbContext.VendorMetrics_Office365_getSharePointActivityUserCounts
                               .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod).OrderBy(x => x.ReportDate).FirstOrDefault().ReportDate;

            DateTime startDate = OldMaxDate.Date.AddDays(-7);
            DateTime endDate = startDate.Date.AddDays(-7);

            var result = _dbContext.VendorMetrics_Office365_getSharePointActivityUserCounts
                                .Where(x => x.CompanyId == companyId && x.ReportDate < startDate.Date && x.ReportDate >= endDate)
                                .GroupBy(x => new { x.CompanyId, x.ReportPeriod })
                                 .Select(u => new
                                 {
                                     SharedExternally = u.Sum(x => x.SharedExternally),
                                     SharedInternally = u.Sum(x => x.SharedInternally),
                                     VisitedPage = u.Sum(x => x.VisitedPage),
                                     ViewedOrEdited = u.Sum(x => x.ViewedOrEdited)
                                 }).FirstOrDefault();
            var oldSum = result.SharedExternally + result.SharedInternally + result.ViewedOrEdited + result.VisitedPage;
            onedriveTrend = ((Total - Convert.ToDouble(oldSum)) / Convert.ToDouble(oldSum)) * 100;
            return onedriveTrend;
        }

        public double GetSkypeTrend(int companyId, int ReportPeriod, double Total)
        {
            double onedriveTrend = 0;
            int targetPeriod = 30;
            if (ReportPeriod != 7)
            {
                targetPeriod = ReportPeriod == 30 ? 90 : 180;
            }
            var OldMaxDate = _dbContext.VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts
                               .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod).OrderBy(x => x.ReportDate).FirstOrDefault().ReportDate;

            DateTime startDate = OldMaxDate.Date.AddDays(-7);
            DateTime endDate = startDate.Date.AddDays(-7);

            var result = _dbContext.VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts
                                .Where(x => x.CompanyId == companyId && x.ReportDate < startDate.Date && x.ReportDate >= endDate)
                                .GroupBy(x => new { x.CompanyId, x.ReportPeriod })
                                 .Select(u => new
                                 {
                                     Organized = u.Sum(x => x.Organized),
                                     Participated = u.Sum(x => x.Participated),
                                     PeerToPeer = u.Sum(x => x.PeerToPeer)
                                 }).FirstOrDefault();
            var oldSum = result.Organized + result.Participated + result.PeerToPeer;
            onedriveTrend = ((Total - Convert.ToDouble(oldSum)) / Convert.ToDouble(oldSum)) * 100;
            return onedriveTrend;
        }

        public double GetTeamsTrend(int companyId, int ReportPeriod, double Total)
        {
            double onedriveTrend = 0;
            int targetPeriod = 30;
            if (ReportPeriod != 7)
            {
                targetPeriod = ReportPeriod == 30 ? 90 : 180;
            }
            var OldMaxDate = _dbContext.VendorMetrics_Office365_getTeamsUserActivityUserCounts
                               .Where(x => x.CompanyId == companyId && x.ReportPeriod == targetPeriod).OrderBy(x => x.ReportDate).FirstOrDefault().ReportDate;

            DateTime startDate = OldMaxDate.Date.AddDays(-7);
            DateTime endDate = startDate.Date.AddDays(-7);

            var result = _dbContext.VendorMetrics_Office365_getTeamsUserActivityUserCounts
                                .Where(x => x.CompanyId == companyId && x.ReportDate < startDate.Date && x.ReportDate >= endDate)
                                .GroupBy(x => new { x.CompanyId, x.ReportPeriod })
                                 .Select(u => new
                                 {
                                     Calls = u.Sum(x => x.Calls),
                                     Meetings = u.Sum(x => x.Meetings),
                                     OtherActions = u.Sum(x => x.OtherActions),
                                     PrivateChatMessages = u.Sum(x => x.PrivateChatMessages),
                                     TeamChatMessages = u.Sum(x => x.TeamChatMessages)
                                 }).FirstOrDefault();
            var oldSum = result.Calls + result.Meetings + result.OtherActions + result.PrivateChatMessages + result.TeamChatMessages;
            onedriveTrend = ((Total - Convert.ToDouble(oldSum)) / Convert.ToDouble(oldSum)) * 100;
            return onedriveTrend;
        }

        #endregion All Trends for Dashboard

        #endregion Dashboard

        #region Details Page

        #region Email Activity

        /// <summary>GetActiveUsers
        /// Retrieve the Email Activity
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>GetOneDriveStorage
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>       
        public VendorMetrics_DetailsPageChartModel GetEmailActivity(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "Email Activity").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getEmailActivityCounts
                                       .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                      .Select(u => new
                                      {
                                          u.Send,
                                          u.Receive,
                                          u.Read,
                                          Date = u.ReportDate
                                      }).ToList();

            if (Activity != null && Activity.Count > 0)
            {
                result.Others = new Others
                {
                    Metrics = "Email Activity",
                    ChartType = metrics.DetailsChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            ValueList[i].Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }

        //TODO: Need to confirm as we dont have the ReportDate in this table

        /// <summary>
        /// Retrieve the Email Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getEmailActivityUserDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getEmailActivityUserDetailModel> GetEmailActivityDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            date = (date == null ? _dbContext.VendorMetrics_Office365_getEmailActivityUserDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivityDate).Max() : date);
            var EmailActivityDetails = _dbContext.VendorMetrics_Office365_getEmailActivityUserDetail
                                            .Where(x => x.CompanyId == companyId && x.LastActivityDate == (date == null ? x.LastActivityDate : date));

            return EmailActivityDetails.Select(c => new
            {
                c.UserPrincipalName,
                c.LastActivityDate,
                c.SendCount,
                c.ReceiveCount,
                c.ReadCount,
            }).ToList().Select(c => new VendorMetrics_Office365_getEmailActivityUserDetailModel
            {
                UserPrincipalName = c.UserPrincipalName,
                LastActivityDate = c.LastActivityDate.ToString("MM/dd/yyyy"),
                SendCount = c.SendCount,
                ReceiveCount = c.ReceiveCount,
                ReadCount = c.ReadCount,
            }).OrderBy(x => x.UserPrincipalName);
        }

        #endregion Email Activity

        #region Active Users

        /// <summary>
        /// Retrieve the Active Users Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>        
        public VendorMetrics_DetailsPageChartModel GetActiveUsers(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "Active Users").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getOffice365ActiveUserCounts
                                       .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                      .Select(u => new
                                      {
                                          u.Exchange,
                                          u.Office365,
                                          u.OneDrive,
                                          u.SharePoint,
                                          u.SkypeForBusiness,
                                          u.Teams,
                                          u.Yammer,
                                          Date = u.ReportDate
                                      }).ToList();
            if (Activity != null && Activity.Count > 0)
            {
                // SetGraphValuesForDetailsPage("Email Activity", 1, (List<object>)Activity, ref result);
                result.Others = new Others
                {
                    Metrics = "Active Users",
                    ChartType = metrics.DetailsChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            ValueList[i].Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }

        /// <summary>
        /// Retrieve the Email Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getOffice365ActiveUserDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getOffice365ActiveUserDetailModel> GetActiveUsersDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {

            //date = (date == null ? _dbContext.VendorMetrics_Office365_getOffice365ActiveUserDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivityDate).Max() : date);

            var EmailActivityDetails = _dbContext.VendorMetrics_Office365_getOffice365ActiveUserDetail
                                            .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod).ToList();

            if (date != null && EmailActivityDetails != null && EmailActivityDetails.Count() > 0)
            {
                EmailActivityDetails = EmailActivityDetails.Where(x =>
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date ||
                    x.OneDriveLastActivityDate == date
                    ).ToList();
            }

            return EmailActivityDetails.Select(c => new VendorMetrics_Office365_getOffice365ActiveUserDetailModel
            {
                UserPrincipalName = c.UserPrincipalName,
                ExchangeLastActivityDate = c.ExchangeLastActivityDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.ExchangeLastActivityDate.ToString("MM/dd/yyyy") : "",
                OneDriveLastActivityDate = c.OneDriveLastActivityDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.OneDriveLastActivityDate.ToString("MM/dd/yyyy") : "",
                SharePointLastActivityDate = c.SharePointLastActivityDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.SharePointLastActivityDate.ToString("MM/dd/yyyy") : "",
                SkypeForBusinessLastActivityDate = c.SkypeForBusinessLastActivityDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.SkypeForBusinessLastActivityDate.ToString("MM/dd/yyyy") : "",
                YammerLastActivityDate = c.YammerLastActivityDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.YammerLastActivityDate.ToString("MM/dd/yyyy") : "",
                TeamsLastActivityDate = c.TeamsLastActivityDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.TeamsLastActivityDate.ToString("MM/dd/yyyy") : "",
                ExchangeLicenseAssignDate = c.ExchangeLicenseAssignDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.ExchangeLicenseAssignDate.ToString("MM/dd/yyyy") : "",
                OneDriveLicenseAssignDate = c.OneDriveLicenseAssignDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.OneDriveLicenseAssignDate.ToString("MM/dd/yyyy") : "",
                SharePointLicenseAssignDate = c.SharePointLicenseAssignDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.SharePointLicenseAssignDate.ToString("MM/dd/yyyy") : "",
                SkypeForBusinessLicenseAssignDate = c.SkypeForBusinessLicenseAssignDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.SkypeForBusinessLicenseAssignDate.ToString("MM/dd/yyyy") : "",
                YammerLicenseAssignDate = c.YammerLicenseAssignDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.YammerLicenseAssignDate.ToString("MM/dd/yyyy") : "",
                TeamsLicenseAssignDate = c.TeamsLicenseAssignDate.ToString("MM/dd/yyyy") != "01/01/1900" ? c.TeamsLicenseAssignDate.ToString("MM/dd/yyyy") : "",
                AssignedProducts = c.AssignedProducts,
            }).OrderBy(x => x.UserPrincipalName);
        }

        #endregion Active Users

        #region Office 365 Activations

        /// <summary>
        /// Retrieve the Office 365 Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>

        public VendorMetrics_DetailsPageChartModel GetOfficeActivation(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "Office Activations").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getOffice365ActivationCounts
                                      .Where(x => x.CompanyId == companyId)
                                      .Select(u => new
                                      {
                                          u.Windows,
                                          u.Android,
                                          u.Mac,
                                          u.iOS,
                                          u.Windows10Mobile
                                      }).ToList();
            if (Activity != null && Activity.Count > 0)
            {
                // SetGraphValuesForDetailsPage("Email Activity", 1, (List<object>)Activity, ref result);
                result.Others = new Others
                {
                    Metrics = "Office Activation",
                    ChartType = metrics.DashboardChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            ValueList[i].Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }


        /// <summary>
        /// Retrieve the office 365 Activation summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getOffice365ActivationsUserDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getOffice365ActivationsUserDetailModel> GetOfficeActivationDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            //date = (date == null ? _dbContext.VendorMetrics_Office365_getOffice365ActivationsUserDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivatedDate).Max() : date);
            var result = _dbContext.VendorMetrics_Office365_getOffice365ActivationsUserDetail
                                            .Where(x => x.CompanyId == companyId && x.LastActivatedDate == (date == null ? x.LastActivatedDate : date));

            return result.Select(c => new
            {
                c.UserPrincipalName,
                c.ProductType,
                c.LastActivatedDate,
                c.Windows,
                c.Mac,
                c.iOS,
                c.Android,
                c.Windows10Mobile,
            }).ToList().Select(c => new VendorMetrics_Office365_getOffice365ActivationsUserDetailModel
            {
                UserPrincipalName = c.UserPrincipalName,
                ProductType = c.ProductType,
                LastActivityDate = c.LastActivatedDate.ToString("MM/dd/yyyy"),
                Windows = c.Windows,
                Mac = c.Mac,
                iOS = c.iOS,
                Android = c.Android,
                Windows10Mobile = c.Windows10Mobile,
            }).OrderBy(x => x.UserPrincipalName);
        }

        #endregion Office 365 Activations

        #region OneDrive Storage

        /// <summary>
        /// Retrieve the OneDriveStorage
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>       
        public VendorMetrics_DetailsPageChartModel GetOneDriveStorage(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "OneDrive Files").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getOneDriveUsageStorage
                                        .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod && x.SiteType == "OneDrive")
                                      .Select(u => new
                                      {
                                          u.StorageUsed,
                                          Date = u.ReportDate
                                      }).ToList();
            if (Activity != null && Activity.Count > 0)
            {

                result.Others = new Others
                {
                    Metrics = "OneDrive Storage",
                    ChartType = metrics.DashboardChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            var value = Convert.ToDouble((item.GetType().GetProperty(property).GetValue(item, null).ToString()));
                            ValueList[i].Add(FormatBytes(value));
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }

        /// <summary>
        /// Retrieve the onedrive storage summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getOneDriveUsageAccountDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getOneDriveUsageAccountDetailModel> GetOneDriveStorageDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            date = (date == null ? _dbContext.VendorMetrics_Office365_getOneDriveUsageAccountDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivityDate).Max() : date);
            var EmailActivityDetails = _dbContext.VendorMetrics_Office365_getOneDriveUsageAccountDetail
                                            .Where(x => x.CompanyId == companyId && x.LastActivityDate == (date == null ? x.LastActivityDate : date));

            return EmailActivityDetails.Select(c => new
            {
                SiteURL = c.SiteURL,
                OwnerDisplayName = c.OwnerDisplayName,
                LastActivityDate = c.LastActivityDate,
                FileCount = c.FileCount,
                ActiveFileCount = c.ActiveFileCount,
                StorageUsed = c.StorageUsed,
                StorageAllocated = c.StorageAllocated
            }).ToList().Select(c => new VendorMetrics_Office365_getOneDriveUsageAccountDetailModel
            {
                SiteURL = c.SiteURL,
                OwnerDisplayName = c.OwnerDisplayName,
                LastActivityDate = c.LastActivityDate.ToString("MM/dd/yyyy"),
                FileCount = c.FileCount,
                ActiveFileCount = c.ActiveFileCount,
                StorageUsed = FormatBytesToMB(Convert.ToDouble(c.StorageUsed)),
                StorageAllocated = FormatBytesToMB(Convert.ToDouble(c.StorageAllocated)),

            }).OrderBy(x => x.OwnerDisplayName);
        }

        #endregion OneDrive Storage

        #region SharePoint Activity

        /// <summary>
        /// Retrieve the Sharepoint Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>

        public VendorMetrics_DetailsPageChartModel GetSharePointActivity(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "SharePoint Files").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getSharePointActivityUserCounts
                                        .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                      .Select(u => new
                                      {
                                          u.VisitedPage,
                                          u.ViewedOrEdited,
                                          u.Synced,
                                          u.SharedExternally,
                                          u.SharedInternally,
                                          Date = u.ReportDate
                                      }).ToList();
            if (Activity != null && Activity.Count > 0)
            {

                result.Others = new Others
                {
                    Metrics = metrics.Name,
                    ChartType = metrics.DetailsChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            ValueList[i].Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }

        /// <summary>
        /// Retrieve the SharePoint Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getSharePointActivityUserDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getSharePointActivityUserDetailModel> GetSharePointActivityDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            date = (date == null ? _dbContext.VendorMetrics_Office365_getSharePointActivityUserDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivityDate).Max() : date);
            var EmailActivityDetails = _dbContext.VendorMetrics_Office365_getSharePointActivityUserDetail
                                            .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod && x.LastActivityDate == (date == null ? x.LastActivityDate : date));

            return EmailActivityDetails.Select(c => new
            {
                c.UserPrincipalName,
                c.LastActivityDate,
                c.ViewedOrEditedFileCount,
                c.SyncedFileCount,
                c.SharedInternallyFileCount,
                c.SharedExternallyFileCount,
                c.VisitedPageCount
            }).ToList().Select(c => new VendorMetrics_Office365_getSharePointActivityUserDetailModel
            {
                UserPrincipalName = c.UserPrincipalName,
                LastActivityDate = c.LastActivityDate.ToString("MM/dd/yyyy"),
                ViewedOrEditedFileCount = c.ViewedOrEditedFileCount,
                SyncedFileCount = c.SyncedFileCount,
                SharedInternallyFileCount = c.SharedInternallyFileCount,
                SharedExternallyFileCount = c.SharedExternallyFileCount,
                VisitedPageCount = c.VisitedPageCount
            }).OrderBy(x => x.UserPrincipalName);
        }

        #endregion Sharepoint Activity

        #region Skype For Business

        /// <summary>
        /// Retrieve the skype for business Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>
        public VendorMetrics_DetailsPageChartModel GetSkypeForBusinessActivity(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "Skype for Business Activity").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts
                                        .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                      .Select(u => new
                                      {
                                          u.PeerToPeer,
                                          u.Organized,
                                          u.Participated,
                                          Date = u.ReportDate
                                      }).ToList();
            if (Activity != null && Activity.Count > 0)
            {

                result.Others = new Others
                {
                    Metrics = metrics.Name,
                    ChartType = metrics.DetailsChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            ValueList[i].Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }

        /// <summary>
        /// Retrieve the skypefor business  Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getSkypeForBusinessActivityUserDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getSkypeForBusinessActivityUserDetailModel> GetSkypeForBusinessActivityDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            date = (date == null ? _dbContext.VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivityDate).Max() : date);
            var EmailActivityDetails = _dbContext.VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail
                                            .Where(x => x.CompanyId == companyId && x.LastActivityDate == (date == null ? x.LastActivityDate : date));

            return EmailActivityDetails.Select(c => new
            {
                c.UserPrincipalName,
                c.LastActivityDate,
                c.TotalPeerToPeerSessionCount,
                c.TotalOrganizedConferenceCount,
                c.TotalParticipatedConferenceCount
            }).ToList().Select(c => new VendorMetrics_Office365_getSkypeForBusinessActivityUserDetailModel
            {
                UserPrincipalName = c.UserPrincipalName,
                LastActivityDate = c.LastActivityDate.ToString("MM/dd/yyyy"),
                TotalPeerToPeerSessionCount = c.TotalPeerToPeerSessionCount,
                TotalOrganizedConferenceCount = c.TotalOrganizedConferenceCount,
                TotalParticipatedConferenceCount = c.TotalParticipatedConferenceCount
            }).OrderBy(x => x.UserPrincipalName);
        }

        #endregion Skype For Business

        #region Teams

        /// <summary>
        /// Retrieve the Teams Activity Count
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <returns>VendorMetrics_DetailsPageChartModel</returns>

        public VendorMetrics_DetailsPageChartModel GetTeamsActivity(int companyId, int ReportPeriod)
        {
            VendorMetrics_DetailsPageChartModel result = new VendorMetrics_DetailsPageChartModel();
            var metrics = _dbContext.VendorMetrics.Where(x => x.Name == "Microsoft Teams Activity").FirstOrDefault();
            var Activity = _dbContext.VendorMetrics_Office365_getTeamsUserActivityUserCounts
                                        .Where(x => x.CompanyId == companyId && x.ReportPeriod == ReportPeriod)
                                      .Select(u => new
                                      {
                                          u.TeamChatMessages,
                                          u.PrivateChatMessages,
                                          u.Calls,
                                          u.Meetings,
                                          u.OtherActions,
                                          Date = u.ReportDate
                                      }).ToList();
            if (Activity != null && Activity.Count > 0)
            {

                result.Others = new Others
                {
                    Metrics = metrics.Name,
                    ChartType = metrics.DetailsChartType
                };

                var properties = Activity[0].GetType().GetProperties().ToList().Select(x => x.Name);
                List<string> values1 = new List<string>();
                List<string> values2 = new List<string>();
                List<List<string>> ValueList = new List<List<string>>();
                foreach (var property in properties)
                {
                    if (property != "Date")
                        ValueList.Add(new List<string> { });
                }
                foreach (var item in Activity)
                {
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (property == "Date")
                        {
                            values1.Add(Convert.ToDateTime(item.GetType().GetProperty(property).GetValue(item, null).ToString()).ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            ValueList[i].Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                            i++;
                        }
                    }
                }
                result.Legends = properties.ToList().Where(x => x != "Date").ToArray();
                result.Props = values1.ToArray();
                result.Values = ValueList;

            }
            return result;
        }

        /// <summary>
        /// Retrieve theTeams  Activity summary based on the selected data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ReportPeriod"></param>
        /// <param name="date"></param>
        /// <returns>IEnumerable<VendorMetrics_Office365_getTeamsUserActivityUserDetailModel></returns>
        public IEnumerable<VendorMetrics_Office365_getTeamsUserActivityUserDetailModel> GetTeamsActivityDetails(int companyId, int ReportPeriod, DateTime? date = null)
        {
            date = (date == null ? _dbContext.VendorMetrics_Office365_getTeamsUserActivityUserDetail.Where(y => y.CompanyId == companyId).Select(x => x.LastActivityDate).Max() : date);

            var EmailActivityDetails = _dbContext.VendorMetrics_Office365_getTeamsUserActivityUserDetail
                                            .Where(x => x.CompanyId == companyId && x.LastActivityDate == (date == null ? x.LastActivityDate : date));
            return EmailActivityDetails.Select(c => new
            {
                c.UserPrincipalName,
                c.LastActivityDate,
                c.TeamChatMessageCount,
                c.PrivateChatMessageCount,
                c.CallCount,
                c.MeetingCount,
                c.HasOtherAction
            }).ToList().Select(c => new VendorMetrics_Office365_getTeamsUserActivityUserDetailModel
            {
                UserPrincipalName = c.UserPrincipalName,
                LastActivityDate = c.LastActivityDate.ToString("MM/dd/yyyy"),
                ChatMessages = c.TeamChatMessageCount + c.PrivateChatMessageCount,
                CallCount = c.CallCount,
                MeetingCount = c.MeetingCount,
                HasOtherAction = c.HasOtherAction
            }).OrderBy(x => x.UserPrincipalName);
        }
        #endregion Teams

        #endregion Details Page

        #region Vendor Metrics Admin & Customer Config    

        #region Admin Config

        /// <summary>
        /// Retrieve vendor metrics Admin configuartions based on the CompanyId
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>IEnumerable<VendorMetricsAdminConfigModel></returns>
        public IEnumerable<VendorMetricsAdminConfigModel> GetVendorMetricsAdminConfigByCompanyId(int CompanyId)
        {

            var metricList = _dbContext.VendorMetrics
                                        .Where(x => x.CanAccess == true).ToList();

            var adminConfig = _dbContext.VendorMetricsAdminConfig
                                        .Where(x => x.CompanyId == CompanyId).ToList();

            if (metricList != null)
            {
                foreach (var metric in metricList)
                {
                    if (adminConfig.Where(x => x.MetricsId == metric.Id).Count() == 0)
                    {

                        var newRecord = new Entities.VendorMetricsAdminConfig
                        {
                            CompanyId = CompanyId,
                            MetricsId = metric.Id,
                            CanAccess = true,
                            IsDeleted = false
                        };
                        _dbContext.Entry(newRecord).State = EntityState.Added;
                        _dbContext.SaveChanges();
                    }
                }
            }


            var result = (from AC in _dbContext.VendorMetricsAdminConfig
                          join VM in _dbContext.VendorMetrics on AC.MetricsId equals VM.Id
                          where AC.CompanyId == CompanyId && VM.CanAccess == true
                          select new
                          {
                              AC.CompanyId,
                              VendorMetricsId = AC.MetricsId,
                              VendorMetricsName = VM.Name,
                              AC.CanAccess,
                              AC.Id,
                              AC.IsDeleted,
                              AC.CreateDate,
                              AC.UpdateDate
                          });

            return result.Select(c => new VendorMetricsAdminConfigModel
            {
                CompanyId = c.CompanyId,
                VendorMetricsId = c.VendorMetricsId,
                VendorMetricsName = c.VendorMetricsName,
                Id = c.Id,
                CanAccess = c.CanAccess,
                IsDeleted = c.IsDeleted,
                CreateDate = c.CreateDate,
                UpdateDate = c.UpdateDate
            }).OrderBy(x => x.VendorMetricsId);
        }

        /// <summary>
        /// Update the configuration of a company by the Admin
        /// </summary>
        /// <param name="CompanyConfigurations"></param>
        /// <returns>IEnumerable<VendorMetricsAdminConfigModel></returns>
        public IEnumerable<VendorMetricsAdminConfigModel> UpdateVendorMetricsAdminConfig(List<VendorMetricsAdminConfigModel> CompanyConfigurations)
        {
            int companyId = 0;
            if (CompanyConfigurations != null)
            {
                companyId = CompanyConfigurations[0].CompanyId;
                foreach (var Companyconfig in CompanyConfigurations)
                {
                    var config = _dbContext.VendorMetricsAdminConfig.Where(x => x.Id == Companyconfig.Id).FirstOrDefault();
                    if (config != null)
                    {
                        config.CanAccess = Companyconfig.CanAccess;
                        config.UpdateDate = DateTime.Now;
                        _dbContext.SaveChanges();
                    }
                }
            }
            return GetVendorMetricsAdminConfigByCompanyId(companyId);
        }

        #endregion Admin Config

        #region Customer Config

        /// <summary>
        /// Get the metric list of the user based on the metrics configured for the company
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns>IEnumerable<VendorMetricsUserConfigModel></returns>
        public IEnumerable<VendorMetricsUserConfigModel> GetVendorMetricsUserConfigByUserId(int CompanyId, int UserId)
        {
            CheckandAddUserConfig(CompanyId,UserId);

            var result = (from UC in _dbContext.VendorMetricsUserConfig
                          join AC in _dbContext.VendorMetricsAdminConfig on CompanyId equals AC.CompanyId
                          join VM in _dbContext.VendorMetrics on UC.MetricsId equals VM.Id
                          where AC.CanAccess == true && UC.UserId == UserId && AC.CompanyId == CompanyId && UC.MetricsId == AC.MetricsId && VM.CanAccess == true
                          select new
                          {
                              UC.Id,
                              UC.UserId,
                              UC.MetricsId,
                              UC.CanAccess,
                              UC.IsDeleted,
                              UC.CreateDate,
                              UC.UpdateDate,
                              VendorMetricsId = VM.Id,
                              VendorMetricsName = VM.Name,
                              AC.CompanyId

                          });

            return result.Select(c => new VendorMetricsUserConfigModel
            {
                UserId = c.UserId,
                VendorMetricsId = c.VendorMetricsId,
                VendorMetricsName = c.VendorMetricsName,
                Id = c.Id,
                CanAccess = c.CanAccess,
                IsDeleted = c.IsDeleted,
                CreateDate = c.CreateDate,
                UpdateDate = c.UpdateDate,
                CompanyId = c.CompanyId
            }).OrderBy(x => x.VendorMetricsId);
        }

        public bool CheckandAddUserConfig(int CompanyId, int UserId)
        {
            var metricList = (from AC in _dbContext.VendorMetricsAdminConfig
                              join VM in _dbContext.VendorMetrics on AC.MetricsId equals VM.Id
                              where AC.CanAccess == true && AC.CompanyId == CompanyId && VM.CanAccess == true
                              select new
                              {
                                  VM.Id,
                                  VM.Name
                              }).ToList();
            var adminConfig = _dbContext.VendorMetricsUserConfig
                                        .Where(x => x.UserId == UserId).ToList();

            if (metricList != null)
            {
                foreach (var metric in metricList)
                {
                    if (adminConfig.Where(x => x.MetricsId == metric.Id).Count() == 0)
                    {

                        var newRecord = new Entities.VendorMetricsUserConfig
                        {
                            UserId = UserId,
                            MetricsId = metric.Id,
                            CanAccess = true,
                            IsDeleted = false
                        };
                        _dbContext.Entry(newRecord).State = EntityState.Added;
                        _dbContext.SaveChanges();
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Update the configuration of a Users to customize the dashboard by the user itself
        /// </summary>
        /// <param name="CompanyConfigurations"></param>
        /// <returns>IEnumerable<VendorMetricsUserConfigModel></returns>
        public IEnumerable<VendorMetricsUserConfigModel> UpdateVendorMetricsUserConfig(List<VendorMetricsUserConfigModel> CompanyConfigurations)
        {
            int UserId = 0;
            int CompanyId = 0;

            if (CompanyConfigurations != null)
            {
                UserId = CompanyConfigurations[0].UserId;
                CompanyId = CompanyConfigurations[0].CompanyId;
                foreach (var Userconfig in CompanyConfigurations)
                {
                    var config = _dbContext.VendorMetricsUserConfig.Where(x => x.Id == Userconfig.Id).FirstOrDefault();
                    if (config != null)
                    {
                        config.CanAccess = Userconfig.CanAccess;
                        config.UpdateDate = DateTime.Now;
                        _dbContext.SaveChanges();
                    }
                }
            }
            return GetVendorMetricsUserConfigByUserId(CompanyId, UserId);
            return null;
        }

        #endregion Customer Config

        #region GetAllVendorMetrics
        public IEnumerable<VendorMetricsModel> GetAllWidgets()
        {
            var metricList = _dbContext.VendorMetrics.ToList();
            return metricList.Select(c => new VendorMetricsModel
            {
                Id = c.Id,
                VendorMetricsName = c.Name,
                Description = c.Description,
                CanAccess = c.CanAccess
            }).OrderBy(x => x.Id);
        }

        public bool SaveWidgetsForAllCompanies(List<VendorMetricsModel> vendorMetrics)
        {
            if (vendorMetrics != null)
            {

                foreach (var metric in vendorMetrics)
                {
                    var dbMetric = _dbContext.VendorMetrics.Where(x => x.Id == metric.Id).FirstOrDefault();
                    dbMetric.CanAccess = metric.CanAccess;
                    _dbContext.SaveChanges();
                }
                //var companies = _dbContext.Companies.ToList();
                //if (companies != null && companies.Count > 0)
                //{
                //    var adminconfigs = _dbContext.VendorMetricsAdminConfig.ToList();

                //    foreach (var company in companies)
                //    {
                //        foreach (var config in vendorMetrics)
                //        {                            
                //            var companyConfig = adminconfigs?.Where(x => x.CompanyId == company.Id && x.MetricsId == config.Id).FirstOrDefault();
                //            if (companyConfig == null)
                //            {
                //                companyConfig = new VendorMetricsAdminConfig
                //                {
                //                    CanAccess = config.CanAccess,
                //                    CompanyId = company.Id,
                //                    CreateDate = DateTime.Now,
                //                    IsDeleted = false,
                //                    MetricsId = config.Id,
                //                    UpdateDate = DateTime.Now
                //                };
                //                _dbContext.VendorMetricsAdminConfig.Add(companyConfig);
                //                _dbContext.Entry(companyConfig).State = EntityState.Added;
                //                _dbContext.SaveChanges();
                //            }
                //            else
                //            {
                //                companyConfig.CanAccess = config.CanAccess;
                //                companyConfig.UpdateDate = DateTime.Now;
                //                companyConfig.IsDeleted = false;
                //                _dbContext.Entry(companyConfig).State = EntityState.Modified;
                //                _dbContext.SaveChanges();
                //            }

                //        }
                //    }
                //    _dbContext.SaveChanges();
                //}
            }
            return true;
        }

        public bool ResetAllCompanies()
        {
            var Metriclist = _dbContext.VendorMetrics.Where(x => x.IsDeleted == false).ToList();
            var companies = _dbContext.Companies.Where(x => x.IsDeleted == false).ToList();
            if (companies?.Count() > 0)
            {
                foreach (var company in companies)
                {
                    if (Metriclist?.Count() > 0)
                    {
                        foreach (var metric in Metriclist)
                        {
                            var companyMetric = _dbContext.VendorMetricsAdminConfig.Where(x => x.MetricsId == metric.Id && x.CompanyId == company.Id && x.IsDeleted == false).FirstOrDefault();
                            if (companyMetric != null)
                            {
                                companyMetric.CanAccess = metric.CanAccess;
                                companyMetric.UpdateDate = DateTime.Now;
                                _dbContext.SaveChanges();
                            }
                            else
                            {
                                companyMetric = new VendorMetricsAdminConfig();
                                companyMetric.CanAccess = metric.CanAccess;
                                companyMetric.CompanyId = company.Id;
                                companyMetric.CreateDate = DateTime.Now;
                                companyMetric.IsDeleted = false;
                                companyMetric.MetricsId = metric.Id;
                                _dbContext.Entry(companyMetric).State = EntityState.Added;
                                _dbContext.SaveChanges();

                            }
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool SetCompanyMetrics(int companyId)
        {
            var Metriclist = _dbContext.VendorMetrics.Where(x => x.IsDeleted == false).ToList();

            if (Metriclist?.Count() > 0)
            {
                foreach (var metric in Metriclist)
                {
                    var companyMetric = _dbContext.VendorMetricsAdminConfig.Where(x => x.MetricsId == metric.Id && x.CompanyId == companyId && x.IsDeleted == false).FirstOrDefault();
                    if (companyMetric != null)
                    {
                        companyMetric.CanAccess = metric.CanAccess;
                        companyMetric.UpdateDate = DateTime.Now;
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        companyMetric = new VendorMetricsAdminConfig();
                        companyMetric.CanAccess = metric.CanAccess;
                        companyMetric.CompanyId = companyId;
                        companyMetric.CreateDate = DateTime.Now;
                        companyMetric.IsDeleted = false;
                        companyMetric.MetricsId = metric.Id;
                        _dbContext.Entry(companyMetric).State = EntityState.Added;
                        _dbContext.SaveChanges();

                    }
                }

            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion GetAllVendorMetrics

        #endregion Vendor Metrics Admin & Customer Config

        #region private methods

        /// <summary>
        /// Converts the Bytes to GB
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string FormatBytes(double bytes)
        {
            //string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            string[] Suffix = { "B", "KB", "MB", "GB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            //return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
            return String.Format("{0:0.##}", dblSByte);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string FormatBytesToMB(double bytes)
        {
            //string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            string[] Suffix = { "B", "KB", "MB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            //return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
            return String.Format("{0:0.##}", dblSByte);
        }

        /// <summary>
        /// Common method to set the graph value object for the normal graph/charts
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="ChartType"></param>
        /// <param name="resultObject"></param>
        /// <param name="OtherProperties"></param>
        /// <param name="_dashboardModel"></param>
        private void SetGraphValues(string metric, int ChartType, object resultObject, OtherProperties OtherProperties, ref VendorMetrics_DashboardModel _dashboardModel)
        {
            var properties = resultObject.GetType().GetProperties().ToList().Select(x => (x.Name == "StorageUsed" ? "Storage Used (GB)" : x.Name));
            List<string> values = new List<string>();
            List<List<string>> valueList = new List<List<string>>();
            foreach (var property in properties)
            {
                var PropertyName = (property == "Storage Used (GB)" ? "StorageUsed" : property);
                values.Add(resultObject.GetType().GetProperty(PropertyName).GetValue(resultObject, null).ToString());
            }
            valueList.Add(values);
            _dashboardModel.Metrics.Add(metric);
            _dashboardModel.ChartType.Add(ChartType);

            _dashboardModel.GraphData.Add(new GraphData
            {
                Legends = properties.ToArray(),
                Props = null,
                Values = valueList,
                Others = OtherProperties
            });

        }

        /// <summary>
        /// Common method to set the graph value object for the special graph/charts
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="ChartType"></param>
        /// <param name="resultObject1"></param>
        /// <param name="OtherProperties"></param>
        /// <param name="_dashboardModel"></param>
        private void SetSpecialGraphValues(string metric, int ChartType, List<OneDriveMetric> resultObject1, OtherProperties OtherProperties, ref VendorMetrics_DashboardModel _dashboardModel)
        {
            //var properties = resultObject1[0].GetType().GetProperties().ToList().Select(x => x.Name);
            var properties = resultObject1[0].GetType().GetProperties().ToList().Select(x => (x.Name == "StorageUsed" ? "Storage Used (GB)" : x.Name));
            List<string> values1 = new List<string>();

            List<List<string>> ValueList = new List<List<string>>();
            foreach (var property in properties)
            {
                if (property != "Date")
                    ValueList.Add(new List<string> { });
            }
            foreach (var item in resultObject1)
            {
                int i = 0;
                foreach (var property in properties)
                {
                    if (property == "Date")
                    {
                        values1.Add(item.GetType().GetProperty(property).GetValue(item, null).ToString());
                    }
                    else
                    {
                        var PropertyName = (property == "Storage Used (GB)" ? "StorageUsed" : property);
                        ValueList[i].Add(item.GetType().GetProperty(PropertyName).GetValue(item, null).ToString());
                        i++;
                    }
                }
            }
            _dashboardModel.Metrics.Add(metric);
            _dashboardModel.ChartType.Add(ChartType);

            _dashboardModel.GraphData.Add(new GraphData
            {
                Legends = properties.ToList().Where(x => x != "Date").ToArray(),
                Props = values1.ToArray(),
                Values = ValueList,
                Others = OtherProperties
            });
        }

        public bool SaveVendorMetricsReportConfigs(VendorMetricsReportConfigsModel _VendorMetricsReportConfigsModel)
        {
            try
            {
                if (_VendorMetricsReportConfigsModel != null)
                {
                    if (_VendorMetricsReportConfigsModel.Id == 0)
                    {
                        var newRecord = new Entities.VendorMetricsReportConfig
                        {
                            CompanyId = _VendorMetricsReportConfigsModel.CompanyId,
                            UserId = _VendorMetricsReportConfigsModel.UserId,
                            ReportName = _VendorMetricsReportConfigsModel.ReportName,
                            ReportPeriod = _VendorMetricsReportConfigsModel.ReportPeriod,
                            Widgets = _VendorMetricsReportConfigsModel.Widgets,
                            ReportFrequency = _VendorMetricsReportConfigsModel.ReportFrequency,
                            DayFrequency = _VendorMetricsReportConfigsModel.DayFrequency,
                            WeekFrequency = _VendorMetricsReportConfigsModel.WeekFrequency,
                            MonthFrequency = _VendorMetricsReportConfigsModel.MonthFrequency,
                            EmailList = _VendorMetricsReportConfigsModel.EmailList,
                            IsDeleted = false,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            Runtime = DateTime.Now
                        };
                        _dbContext.Entry(newRecord).State = EntityState.Added;
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        var config = _dbContext.VendorMetricsReportConfig.Where(x => x.Id == _VendorMetricsReportConfigsModel.Id).FirstOrDefault();
                        if (config != null)
                        {
                            config.CompanyId = _VendorMetricsReportConfigsModel.CompanyId;
                            config.UserId = _VendorMetricsReportConfigsModel.UserId;
                            config.ReportName = _VendorMetricsReportConfigsModel.ReportName;
                            config.ReportPeriod = _VendorMetricsReportConfigsModel.ReportPeriod;
                            config.Widgets = _VendorMetricsReportConfigsModel.Widgets;
                            config.ReportFrequency = _VendorMetricsReportConfigsModel.ReportFrequency;
                            config.DayFrequency = _VendorMetricsReportConfigsModel.DayFrequency;
                            config.WeekFrequency = _VendorMetricsReportConfigsModel.WeekFrequency;
                            config.MonthFrequency = _VendorMetricsReportConfigsModel.MonthFrequency;
                            config.EmailList = _VendorMetricsReportConfigsModel.EmailList;
                            config.UpdateDate = DateTime.Now;
                            config.Runtime = DateTime.Now;
                            config.IsDeleted = _VendorMetricsReportConfigsModel.IsDeleted;
                            _dbContext.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<VendorMetricsReportConfigsModel> GetReportConfigBasedOnId(int Id)
        {

            var result = (from VM in _dbContext.VendorMetricsReportConfig
                          where VM.Id == Id && VM.IsDeleted == false
                          select new VendorMetricsReportConfigsModel
                          {
                              Id = VM.Id,
                              CompanyId = VM.CompanyId,
                              UserId = VM.UserId,
                              ReportName = VM.ReportName,
                              ReportPeriod = VM.ReportPeriod,
                              Widgets = VM.Widgets,
                              ReportFrequency = VM.ReportFrequency,
                              DayFrequency = VM.DayFrequency,
                              WeekFrequency = VM.WeekFrequency,
                              MonthFrequency = VM.MonthFrequency,
                              EmailList = VM.EmailList,
                          });
            return result;
        }

        public IEnumerable<VendorMetricsReportConfigsModel> GetReportConfig(int userId)
        {

            var result = (from VM in _dbContext.VendorMetricsReportConfig
                          where VM.IsDeleted == false && VM.UserId == userId
                          select new VendorMetricsReportConfigsModel
                          {
                              Id = VM.Id,
                              CompanyId = VM.CompanyId,
                              UserId = VM.UserId,
                              ReportName = VM.ReportName,
                              ReportPeriod = VM.ReportPeriod,
                              Widgets = VM.Widgets,
                              ReportFrequency = VM.ReportFrequency,
                              DayFrequency = VM.DayFrequency,
                              WeekFrequency = VM.WeekFrequency,
                              MonthFrequency = VM.MonthFrequency,
                              EmailList = VM.EmailList,
                          });
            return result;
        }
        #endregion private methods

    }
}
