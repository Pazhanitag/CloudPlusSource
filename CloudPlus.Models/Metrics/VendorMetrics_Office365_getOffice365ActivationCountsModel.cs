using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getOffice365ActivationCountsModel
    {
        public int CompanyId { get; set; }
        public DateTime ReportRefreshDate { get; set; }
        public string ProductType { get; set; }
        public int Windows { get; set; }
        public int Mac { get; set; }
        public int Android { get; set; }
        public int iOS { get; set; }
        public int Windows10Mobile { get; set; }
        public DateTime LastReportRetrieval { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
