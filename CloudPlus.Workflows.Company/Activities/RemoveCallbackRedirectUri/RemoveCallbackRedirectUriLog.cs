namespace CloudPlus.Workflows.Company.Activities.RemoveCallbackRedirectUri
{
    public class RemoveCallbackRedirectUriLog : IRemoveCallbackRedirectUriLog
    {
        public string Uri { get; set; }
        public int ClientDbId { get; set; }
    }
}
