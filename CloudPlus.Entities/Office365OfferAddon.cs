using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities.Office365
{
    public class Office365OfferAddon : IBaseEntity
    {
        public Office365OfferAddon()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Office365AddonServiceIdentifier { get; set; }
        public string Office365AddonServiceName { get; set; }
        public string Office365ParentIdentifier { get; set; }
    }
}
