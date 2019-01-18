using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getOneDriveUsageStorageModel
    {
        public int CompanyId { get; set; }
        public int ReportPeriod { get; set; }
        public DateTime ReportRefreshDate { get; set; }
        public string SiteType { get; set; }
        public double StorageUsed { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime LastReportRetrieval { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
