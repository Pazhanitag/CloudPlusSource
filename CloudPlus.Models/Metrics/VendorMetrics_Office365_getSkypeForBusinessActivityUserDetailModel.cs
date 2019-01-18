using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using Newtonsoft.Json;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_Office365_getSkypeForBusinessActivityUserDetailModel
    {
        [JsonProperty(PropertyName = "User Name")]
        public string UserPrincipalName { get; set; }
        [JsonProperty(PropertyName = "Last Activity Date (UTC)")]
        public string LastActivityDate { get; set; }
        [JsonProperty(PropertyName = "Peer to peer")]
        public int TotalPeerToPeerSessionCount { get; set; }
        [JsonProperty(PropertyName = "Organized Conferences")]
        public int TotalOrganizedConferenceCount { get; set; }
        [JsonProperty(PropertyName = "Participated in Conferences")]
        public int TotalParticipatedConferenceCount { get; set; }
      
    }
}
