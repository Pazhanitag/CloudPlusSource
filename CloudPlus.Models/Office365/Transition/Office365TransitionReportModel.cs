using System.Collections.Generic;

namespace CloudPlus.Models.Office365.Transition
{
    public class Office365TransitionReportModel
    {
        public Office365TransitionReportModel()
        {
            Domains = new List<string>();
            Subscriptions = new List<Office365TransitionSubscriptionReportModel>();
            DeletedUsersSucceed = new List<string>();
            DeletedUsersFailed = new List<string>();
            KeepLicensesUsers = new List<string>();
            AdminUsersSucceed = new List<string>();
            AdminUsersFailed = new List<string>();
            LicenseUsersSucceed = new List<string>();
            LicenseUsersFailed = new List<string>();
        }

        public List<string> Domains { get; set; }
        public List<Office365TransitionSubscriptionReportModel> Subscriptions { get; set; }
        public List<string> DeletedUsersSucceed { get; set; }
        public List<string> DeletedUsersFailed { get; set; }
        public List<string> KeepLicensesUsers { get; set; }
        public List<string> AdminUsersSucceed { get; set; }
        public List<string> AdminUsersFailed { get; set; }
        public List<string> LicenseUsersSucceed { get; set; }
        public List<string> LicenseUsersFailed { get; set; }
    }
}
