using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetricsAdminConfigModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int VendorMetricsId { get; set; }
        public string VendorMetricsName { get; set; }
        public bool CanAccess { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
