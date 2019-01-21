using CloudPlus.Enums.Provisions;
using CloudPlus.Models.Provisions;
using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.User
{
    public class UserServiceViewModel
    {
        public int ProductId { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string AssignedLicense { get; set; }
        public List<AssignedServicesModel> AssignedLicenses { get; set; }
        public UserProvisioningStatus Status { get; set; }
        public string StatusToDisplay
        {
            get
            {
                return Status.ToString();
            }
        }
    }
}