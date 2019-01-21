using CloudPlus.Enums.Provisions;
using System.Collections.Generic;

namespace CloudPlus.Models.Provisions
{
    public class ProvisionedServiceModel
    {
        public int ProductId { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string AssignedLicense { get; set; } = "No package assigned";
        public List<AssignedServicesModel> AssignedLicenses { get; set; }
        public bool IsDomainFederated { get; set; }//TODO
        public UserProvisioningStatus Status { get; set; }
    }
}
