using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getOffice365ActivationsUserDetailModel
    {
        [JsonProperty(PropertyName = "User Name")]
        public string UserPrincipalName { get; set; }
        //public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "Product License")]
        public string ProductType { get; set; }
        [JsonProperty(PropertyName = "Last Activity Date (UTC)")]
        public string LastActivityDate { get; set; }
        public int Windows { get; set; }
        public int Mac { get; set; }
        public int iOS { get; set; }
        public int Android { get; set; }
        [JsonProperty(PropertyName = "Windows 10 Mobile")]
        public int Windows10Mobile { get; set; }
    }
}
