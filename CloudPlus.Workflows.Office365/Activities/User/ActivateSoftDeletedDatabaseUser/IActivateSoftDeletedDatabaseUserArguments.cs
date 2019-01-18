namespace CloudPlus.Workflows.Office365.Activities.User.ActivateSoftDeletedDatabaseUser
{
    public interface IActivateSoftDeletedDatabaseUserArguments
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
