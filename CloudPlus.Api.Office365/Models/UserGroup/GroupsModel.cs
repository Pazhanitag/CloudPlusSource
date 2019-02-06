using CloudPlus.Api.Office365.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Office365.Models.UserGroup
{
    public class GroupsModel : IOffice365Model
    {
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }
    }
}