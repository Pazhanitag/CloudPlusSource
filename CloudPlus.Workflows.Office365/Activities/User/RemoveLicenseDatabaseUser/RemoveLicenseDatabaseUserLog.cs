namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser
{
    public class RemoveLicenseDatabaseUserLog : IRemoveLicenseDatabaseUserLog
    {
        public string UserPrincipalName { get; set; }
        public string CloudPlusProductIdentifier { get; set; }
    }
}
