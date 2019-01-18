namespace CloudPlus.Workflows.Office365.Activities.User.GetUserRoles
{
    public interface IGetUserRolesArguments
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
