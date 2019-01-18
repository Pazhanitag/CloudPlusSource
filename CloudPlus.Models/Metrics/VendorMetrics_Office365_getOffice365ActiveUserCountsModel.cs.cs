using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getOffice365ActiveUserCountsModel
    {
        public int CompanyId { get; set; }
        public int ReportPeriod { get; set; }
        public DateTime ReportRefreshDate { get; set; }
        public int Office365 { get; set; }
        public int Exchange { get; set; }
        public int OneDrive { get; set; }
        public int SharePoint { get; set; }
        public int SkypeForBusiness { get; set; }
        public int Yammer { get; set; }
        public int Teams { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime LastReportRetrieval { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
