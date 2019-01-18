using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities
{
    public class Office365InCompatibleService
    {
        public Office365InCompatibleService()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }

        public string ServiceId { get; set; }
        public string CompatibleServiceId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
