using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getOffice365ActiveUserDetailModel
    {
       
        [JsonProperty(PropertyName = "User Name")]
        public string UserPrincipalName { get; set; }
        [JsonProperty(PropertyName = "Exchange Last Activity Date")]
        public string ExchangeLastActivityDate { get; set; }
        [JsonProperty(PropertyName = "OneDrive Last Activity Date")]
        public string OneDriveLastActivityDate { get; set; }
        [JsonProperty(PropertyName = "SharePoint Last Activity Date")]
        public string SharePointLastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Skype For Business Last Activity Date")]
        public string SkypeForBusinessLastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Yammer Last Activity Date")]
        public string YammerLastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Teams Las tActivity Date")]
        public string TeamsLastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Exchange License Assign Date")]
        public string ExchangeLicenseAssignDate { get; set; }
        [JsonProperty(PropertyName = "OneDrive License Assign Date")]
        public string OneDriveLicenseAssignDate { get; set; }
        [JsonProperty(PropertyName = "SharePoint License Assign Date")]
        public string SharePointLicenseAssignDate { get; set; }
        [JsonProperty(PropertyName = "Skype For Business License Assign Date")]
        public string SkypeForBusinessLicenseAssignDate { get; set; }
        [JsonProperty(PropertyName = "Yammer License Assign Date")]
        public string YammerLicenseAssignDate { get; set; }
        [JsonProperty(PropertyName = "Teams License Assign Date")]
        public string TeamsLicenseAssignDate { get; set; }
        [JsonProperty(PropertyName = "Assigned Products")]
        public string AssignedProducts { get; set; }


    }
}
