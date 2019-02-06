using CloudPlus.Api.Office365.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Office365.Models.UserGroup
{
    public class DistributionGroupMembersModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "DistributionGroupName")]
        public string DistributionGroupName { get; set; }
        [Office365Name(O365PropertyName = "MemberSMTPAddress")]
        public string MemberSMTPAddress { get; set; }
       
    }
}