using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getTeamsUserActivityUserDetailModel
    {
        [JsonProperty(PropertyName = "User Name")]
        public string UserPrincipalName { get; set; }
        [JsonProperty(PropertyName = "Last Activity Date (UTC)")]
        public string LastActivityDate { get; set; }
        [JsonProperty(PropertyName = "chat Messages")]
        public int ChatMessages { get; set; }
        [JsonProperty(PropertyName = "Calls")]
        public int CallCount { get; set; }
        [JsonProperty(PropertyName = "Meetings")]
        public int MeetingCount { get; set; }
        [JsonProperty(PropertyName = "Channel Messages")]
        public string HasOtherAction { get; set; }
    }
}
