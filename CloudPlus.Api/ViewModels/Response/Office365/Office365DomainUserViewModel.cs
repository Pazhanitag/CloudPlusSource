using CloudPlus.Enums.Provisions;
using CloudPlus.Models.Provisions;
using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Office365
{
    public class Office365DomainUserViewModel
    {
        public int Id { get; set; }
        public string ProfilePicture { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsProvisioned { get; set; }
        public string AssignedLicense { get; set; }
        public List<AssignedServicesModel> AssignedLicenses { get; set; }
    }
}