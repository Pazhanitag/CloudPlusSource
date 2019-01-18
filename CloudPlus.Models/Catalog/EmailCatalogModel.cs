using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Catalog
{
    public class EmailCatalogModel
    {
        public string userId { get; set; }
        public string catalogId { get; set; }
        public string companyId { get; set; }
        public string recipients { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
