namespace CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri
{
    public interface IAddCallbackRedirectUriLog
    {
        string RedirectUri { get; set; }
        string SilentRedirectUri { get; set; }
        string PostLogoutRedirectUri { get; set; }
        int ClientDbId { get; set; }
    }
}
