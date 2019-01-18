using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities
{
    public class VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail : IBaseEntity
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int ReportPeriod { get; set; }
        [Required]
        public DateTime ReportRefreshDate { get; set; }
        public string UserPrincipalName { get; set; }
        public DateTime DeletedDate { get; set; }       
        public DateTime LastActivityDate { get; set; }
        public int TotalPeerToPeerSessionCount { get; set; }
        public int TotalOrganizedConferenceCount { get; set; }
        public int TotalParticipatedConferenceCount { get; set; }
        public DateTime PeerToPeerLastActivityDate { get; set; }
        public DateTime OrganizedConferenceLastActivityDate { get; set; }
        public DateTime ParticipatedConferenceLastActivityDate { get; set; }
        public int PeerToPeerIMCount { get; set; }
        public int PeerToPeerAudioCount { get; set; }
        public int PeerToPeerAudioMinutes { get; set; }
        public int PeerToPeerVideoCount { get; set; }
        public int PeerToPeerVideoMinutes { get; set; }
        public int PeerToPeerAppSharingCount { get; set; }
        public int PeerToPeerFileTransferCount { get; set; }
        public int OrganizedConferenceIMCount { get; set; }
        public int OrganizedConferenceAudioVideoCount { get; set; }
        public int OrganizedConferenceAudioVideoMinutes { get; set; }
        public int OrganizedConferenceAppSharingCount { get; set; }
        public int OrganizedConferenceWebSharingCount { get; set; }
        public int OrganizedConferenceDialInOut3rdPartyCount { get; set; }
        public int OrganizedConferenceDialInOutMicrosoftCount { get; set; }
        public int OrganizedConferenceDialInMicrosoftMinutes { get; set; }
        public int OrganizedConferenceDialOutMicrosoftMinutes { get; set; }
        public int ParticipatedConferenceIMCount { get; set; }
        public int ParticipatedConferenceAudioVideoCount { get; set; }
        public int ParticipatedConferenceAudioVideoMinutes { get; set; }
        public int ParticipatedConferenceAppSharingCount { get; set; }
        public int ParticipatedConferenceWebCount { get; set; }
        public int ParticipatedConferenceDialInOut3rdPartyCount { get; set; }
        public string AssignedProducts { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime LastReportRetrieval { get; set; }

        public int Id
        { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue(false)]
        public bool IsDeleted
        { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime CreateDate
        { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime UpdateDate
        { get; set; }
    }
}
