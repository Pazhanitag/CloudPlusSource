namespace CloudPlus.Workflows.Office365.Activities.Identity.RemoveCallbackRedirectUri
{
    public interface IRemoveCallbackRedirectUriArguments
    {
        string Uri { get; set; }
        int ClientDbId { get; set; }
    }
}
