namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserRemoveLicenseCommand
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
