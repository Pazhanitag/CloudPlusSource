using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365UserChangeLicenseViewModel
    {
        public int CompanyId { get; set; }
        public string UserPrincipalName { get; set; }
        public string RemoveCloudPlusProductIdentifier { get; set; }
        public string AssignCloudPlusProductIdentifier { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}
