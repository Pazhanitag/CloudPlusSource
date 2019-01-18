using System;
using System.Collections.Generic;
using System.ComponentModel;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getEmailActivityUserDetailModel
    {
   
        [JsonProperty(PropertyName = "User Name")]
        public string UserPrincipalName { get; set; }
        [JsonProperty(PropertyName = "Last Activity Date (UTC)")]
        public string LastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Send Actions")]
        public int SendCount { get; set; }
        [JsonProperty(PropertyName = "Receive Actions")]
        public int ReceiveCount { get; set; }
        [JsonProperty(PropertyName = "Read Actions")]
        public int ReadCount { get; set; }

    }
}
