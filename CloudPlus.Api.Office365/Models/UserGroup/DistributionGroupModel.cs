using CloudPlus.Api.Office365.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Office365.Models.UserGroup
{
    public class DistributionGroupModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "DistributionGroupName")]
        public string DistributionGroupName { get; set; }
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }

        [Office365Name(O365PropertyName = "ManagerSMTPAddress")]
        public string ManagerSMTPAddress { get; set; }
        [Office365Name(O365PropertyName = "MemberJoinPolicy")]
        public string MemberJoinPolicy { get; set; }
        [Office365Name(O365PropertyName = "GroupSMTPAddress")]
        public string GroupSMTPAddress { get; set; }

    }
}