using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getEmailActivityCountsModel
    {
        public int CompanyId { get; set; }
        public int ReportPeriod { get; set; }
        public DateTime ReportRefreshDate { get; set; }
        public int Send { get; set; }
        public int Receive { get; set; }
        public int Read { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime LastReportRetrieval { get; set; }

    }
}
