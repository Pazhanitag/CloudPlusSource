using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities
{
    public class VendorMetricsReportConfig : IBaseEntity
    {

        public VendorMetricsReportConfig()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string ReportName { get; set; }
        public int ReportPeriod { get; set; }
        public string Widgets { get; set; }
        public int ReportFrequency { get; set; }
        public DateTime Runtime { get; set; }
        public int? DayFrequency { get; set; }
        public int? WeekFrequency { get; set; }
        public int? MonthFrequency { get; set; }
        public string EmailList { get; set; }
        public DateTime? LastRunTime { get; set; }
        public string LatRunStatus { get; set; }
        public DateTime? NextRunTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

