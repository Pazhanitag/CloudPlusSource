using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.Office365.Models.UserGroup
{
    public class GroupsListModel
    {
        public string ObjectId { get; set; }
        public string DisplayName { get; set; }
        public string GroupType { get; set; }
    }
}