namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser
{
    public interface IRemoveLicensePartnerPortalUserLog
    {
        string Office365CustomerId { get; set; }
        string Office365UserId { get; set; }
        string Office365OfferSku { get; set; }
    }
}
