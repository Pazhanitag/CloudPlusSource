namespace CloudPlus.Workflows.Office365.Activities.User.SetImmutableId
{
    public interface SetImmutableIdArguments
    {
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
    }
}