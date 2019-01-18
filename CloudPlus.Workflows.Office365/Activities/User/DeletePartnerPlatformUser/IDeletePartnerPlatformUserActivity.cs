using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser
{
    public interface IDeletePartnerPlatformUserActivity 
        : Activity<IDeletePartnerPlatformUserArguments, IDeletePartnerPlatformUserLog>
    {
    }
}
