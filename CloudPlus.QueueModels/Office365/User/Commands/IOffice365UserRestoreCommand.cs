namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserRestoreCommand
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
