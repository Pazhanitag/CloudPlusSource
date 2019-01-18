using System.Collections.Generic;
using CloudPlus.Api.Office365.Attributes;

namespace CloudPlus.Api.Office365.Models.User
{
    public class UserRolesModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }
        [Office365Name(O365PropertyName = "UserPrincipalName")]
        public string UserPrincipalName { get; set; }
        [Office365Name(O365PropertyName = "Roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}