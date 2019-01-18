namespace CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri
{
    public class AddCallbackRedirectUriLog : IAddCallbackRedirectUriLog
    {
        public string RedirectUri { get; set; }
        public string SilentRedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public int ClientDbId { get; set; }
    }
}
