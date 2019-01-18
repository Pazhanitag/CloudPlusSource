namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser
{
    public interface IRemoveLicenseDatabaseUserArguments
    {
        string UserPrincipalName { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
