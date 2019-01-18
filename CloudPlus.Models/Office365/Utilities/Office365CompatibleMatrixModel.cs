using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Office365.Utilities
{
    public class Office365CompatibleMatrixModel
    {

        public List<Office365Services> licenses { get; set; }
        public List<Office365CompatabileMatrix> compatibleLicensesForEachLicense { get; set; }

    }
   
    public class Office365CompatabileMatrix
    {
        public Office365Services selectedLicense { get; set; }
        public List<Office365Services> compatibleLicenses { get; set; }
    }
    public class Office365Services
    {
        public string cloudPlusProductIdentifier { get; set; }
        public string offerName { get; set; }
    }

    public class Office365CompatabileServices
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string CompapatibleID { get; set; }
        public string CompatibleName { get; set; }
    }
  
    public class MapProductItem
    {
        public string ProductIdentifier { get; set; }
        public int? ProductItemID { get; set; }
        public string ProductItemName { get; set; }
    }
}
