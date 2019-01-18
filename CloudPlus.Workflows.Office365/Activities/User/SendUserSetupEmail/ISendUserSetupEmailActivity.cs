using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.SendUserSetupEmail
{
    public interface ISendUserSetupEmailActivity : ExecuteActivity<ISendUserSetupEmailArguments>
    {
    }
}
