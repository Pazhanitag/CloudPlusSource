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
    public class VendorMetrics_Office365_getEmailActivityCounts : IBaseEntity
    {

        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int ReportPeriod { get; set; }
        [Required]
        public DateTime ReportRefreshDate { get; set; }
        public int Send { get; set; }
        public int Receive { get; set; }
        public int Read { get; set; }
        public DateTime ReportDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime LastReportRetrieval { get; set; }

        public int Id
        { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue(false)]
        public bool IsDeleted
        { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime CreateDate
        { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime UpdateDate
        { get; set; }
    }
}
