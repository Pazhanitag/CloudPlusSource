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
    public class VendorMetricsAdminConfig : IBaseEntity
    {

        public VendorMetricsAdminConfig()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int MetricsId { get; set; }
        public bool CanAccess { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
