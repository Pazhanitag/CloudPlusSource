namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365HardDeleteUserCommand
    {
        string UserPrincipalName { get; set; }
        string Office365CustomerId { get; set; }
    }
}