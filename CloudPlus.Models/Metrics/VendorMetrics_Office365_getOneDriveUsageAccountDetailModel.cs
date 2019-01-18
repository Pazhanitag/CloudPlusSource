using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getOneDriveUsageAccountDetailModel
    {
        
        [JsonProperty(PropertyName = "URL")]
        public string SiteURL { get; set; }
        [JsonProperty(PropertyName = "Owner")]
        public string OwnerDisplayName { get; set; }
        [JsonProperty(PropertyName = "Last Activity Date (UTC)")]
        public string LastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Files")]
        public int FileCount { get; set; }
        [JsonProperty(PropertyName = "Active Files")]
        public int ActiveFileCount { get; set; }
        [JsonProperty(PropertyName = "Storage Userd (MB)")]
        public string StorageUsed { get; set; }
        [JsonProperty(PropertyName = "Storage Allocated (MB)")]
        public string StorageAllocated { get; set; }
    }
}
