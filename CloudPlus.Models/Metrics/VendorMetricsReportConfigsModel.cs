using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetricsReportConfigsModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string ReportName { get; set; }
        public int ReportPeriod { get; set; }
        public string Widgets { get; set; }
        public int ReportFrequency { get; set; }        
        public int? DayFrequency { get; set; }
        public int? WeekFrequency { get; set; }
        public int? MonthFrequency { get; set; }
        public string EmailList { get; set; }
        public bool IsDeleted { get; set; }

    }
}
