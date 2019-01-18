namespace CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri
{
    public interface IAddCallbackRedirectUriArguments
    {
        string Uri { get; set; }
        int ClientDbId { get; set; }
    }
}
