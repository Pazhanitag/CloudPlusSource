namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser
{
    public interface IRemoveLicenseDatabaseUserLog
    {
        string UserPrincipalName { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
