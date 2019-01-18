namespace CloudPlus.QueueModels.Office365.Transition.Commands
{
    public interface IOffice365TransitionDeletePartnerPlatformUserCommand
    {
        string Office365CustomerId { get; set; }
        string Office365UserId { get; set; }
    }
}
