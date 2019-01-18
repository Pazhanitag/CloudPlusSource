using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetricsConfigurationModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool ShowActiveUsers { get; set; }
        public bool ShowEmailActivity { get; set; }
        public bool ShowOneDriveStorage { get; set; }
        public bool ShowSharePointActivity { get; set; }
        public bool ShowSkypeForBusinessActivity { get; set; }
        public bool ShowOfficeActivations { get; set; }
        public bool ShowTeamsActivity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
