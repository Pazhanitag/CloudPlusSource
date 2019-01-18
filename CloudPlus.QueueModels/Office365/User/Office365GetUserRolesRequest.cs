namespace CloudPlus.QueueModels.Office365.User
{
    public class Office365GetUserRolesRequest : IOffice365GetUserRolesRequest
    {
        public int CompanyId { get; set; }
        public string UserPrincipalName { get; set; }
    }
}
