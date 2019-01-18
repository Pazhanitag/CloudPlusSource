using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.RestorePartnerPlatformUser
{
    public interface IRestorePartnerPlatformUserActivity : Activity<IRestorePartnerPlatformUserArguments, IRestorePartnerPlatformUserLog>
    {
    }
}
