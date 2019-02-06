using CloudPlus.Api.Office365.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Office365.Models.UserGroup
{
    public class RemoveDistributionGroupModel : IOffice365Model
    {

        [Office365Name(O365PropertyName = "DistributionGroupName")]
        public string DistributionGroupName { get; set; }
    }
}