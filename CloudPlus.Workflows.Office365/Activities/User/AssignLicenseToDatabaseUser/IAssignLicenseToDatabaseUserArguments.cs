namespace CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser
{
    public interface IAssignLicenseToDatabaseUserArguments
    {
        string UserPrincipalName { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
