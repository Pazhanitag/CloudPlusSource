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
    public class VendorMetrics_Office365_getOffice365ActiveUserDetail : IBaseEntity
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
        public bool HasExchangeLicense { get; set; }      
        public bool HasOneDriveLicense { get; set; }        
        public bool HasSharePointLicense { get; set; }       
        public bool HasSkypeForBusinessLicense { get; set; }       
        public bool HasYammerLicense { get; set; }      
        public bool HasTeamsLicense { get; set; }       
        public DateTime ExchangeLastActivityDate { get; set; }      
        public DateTime OneDriveLastActivityDate { get; set; }       
        public DateTime SharePointLastActivityDate { get; set; }    
        public DateTime SkypeForBusinessLastActivityDate { get; set; }        
        public DateTime YammerLastActivityDate { get; set; }      
        public DateTime TeamsLastActivityDate { get; set; }       
        public DateTime ExchangeLicenseAssignDate { get; set; }      
        public DateTime OneDriveLicenseAssignDate { get; set; }     
        public DateTime SharePointLicenseAssignDate { get; set; }       
        public DateTime SkypeForBusinessLicenseAssignDate { get; set; }        
        public DateTime YammerLicenseAssignDate { get; set; }      
        public DateTime TeamsLicenseAssignDate { get; set; }      
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
