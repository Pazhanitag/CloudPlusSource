using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser
{
    public interface ICreatePartnerPlatformUserActivity : Activity<ICreatePartnerPlatformUserArguments, ICreatePartnerPlatformUserLog>
    {
    }
}
