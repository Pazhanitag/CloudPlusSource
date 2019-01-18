using System;
using CloudPlus.Enums.Office365;

namespace CloudPlus.Entities.Office365
{
    public class Office365Domain : IBaseEntity
    {
        public Office365Domain()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
            IsFederated = false;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string DomainName { get; set; }
        public bool IsFederated { get; set; }
        public Office365DomainStatus Office365DomainStaus { get; set; }

        public Office365Customer Office365Customer { get; set; }
    }
}
