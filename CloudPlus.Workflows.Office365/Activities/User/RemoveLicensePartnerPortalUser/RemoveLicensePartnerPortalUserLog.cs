namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser
{
    public class RemoveLicensePartnerPortalUserLog : IRemoveLicensePartnerPortalUserLog
    {
        public string Office365CustomerId { get; set; }
        public string Office365UserId { get; set; }
        public string Office365OfferSku { get; set; }
    }
}
