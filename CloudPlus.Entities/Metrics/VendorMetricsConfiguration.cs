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
    public class VendorMetricsConfiguration : IBaseEntity
    {

        public VendorMetricsConfiguration()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
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
