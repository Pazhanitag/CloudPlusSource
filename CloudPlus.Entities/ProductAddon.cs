using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities.Catalog
{
    public class ProductAddon : IBaseEntity
    {
        public ProductAddon()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ProductItemIdentifier { get; set; }
        public string ProductItemAddonIdentifier { get; set; }

    }
}
