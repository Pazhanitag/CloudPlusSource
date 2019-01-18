namespace CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser
{
    public interface ICreateDatabaseUserArguments
    {
        string UserPrincipalName { get; set; }
        string Office365UserId { get; set; }
    }
}
