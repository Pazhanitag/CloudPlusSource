using CloudPlus.Api.Office365.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Office365.Models.UserGroup
{
    public class SecurityGroupMemberModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "CustomerO365Domain")]
        public string CustomerO365Domain { get; set; }

        [Office365Name(O365PropertyName = "SecurityGroupName")]
        public string SecurityGroupName { get; set; }

        [Office365Name(O365PropertyName = "UserSMTPAddress")]
        public string UserSMTPAddress { get; set; }
       
    }
}