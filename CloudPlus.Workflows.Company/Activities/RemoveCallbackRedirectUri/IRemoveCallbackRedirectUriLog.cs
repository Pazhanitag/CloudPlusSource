namespace CloudPlus.Workflows.Company.Activities.RemoveCallbackRedirectUri
{
    public interface IRemoveCallbackRedirectUriLog
    {
        string Uri { get; set; }
        int ClientDbId { get; set; }
    }
}
