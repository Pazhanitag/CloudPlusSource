namespace CloudPlus.QueueModels.Office365.User
{
    public interface IOffice365GetUserRolesRequest
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
