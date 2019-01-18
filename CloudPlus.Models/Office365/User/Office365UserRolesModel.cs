using System.Collections.Generic;

namespace CloudPlus.Models.Office365.User
{
    public class Office365UserRolesModel
    {
        public string UserPrincipalName { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
