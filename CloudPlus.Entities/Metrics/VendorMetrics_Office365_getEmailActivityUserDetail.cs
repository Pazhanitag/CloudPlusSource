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
    public class VendorMetrics_Office365_getEmailActivityUserDetail : IBaseEntity
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int ReportPeriod { get; set; }
        [Required]
        public DateTime ReportRefreshDate { get; set; }
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public DateTime DeletedDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int SendCount { get; set; }
        public int ReceiveCount { get; set; }
        public int ReadCount { get; set; }
        public string AssignedProducts { get; set; }
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
