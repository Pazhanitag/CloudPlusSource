using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getSharePointActivityUserDetailModel
    {
        
        [JsonProperty(PropertyName = "User Name")]
        public string UserPrincipalName { get; set; }
        [JsonProperty(PropertyName = "Last Activity Date (UTC)")]
        public string LastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Files Viewed or Edited")]
        public int ViewedOrEditedFileCount { get; set; }
        [JsonProperty(PropertyName = "Files Synced")]
        public int SyncedFileCount { get; set; }
        [JsonProperty(PropertyName = "Files Shared Internally")]
        public int SharedInternallyFileCount { get; set; }
        [JsonProperty(PropertyName = "Files Shared Externally")]
        public int SharedExternallyFileCount { get; set; }
        [JsonProperty(PropertyName = "Pages Visited")]
        public int VisitedPageCount { get; set; }
           }
}
