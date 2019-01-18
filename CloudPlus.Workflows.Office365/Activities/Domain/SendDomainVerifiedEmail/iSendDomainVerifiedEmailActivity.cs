using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Domain.SendDomainVerifiedEmail
{
    public interface ISendDomainVerifiedEmailActivity : ExecuteActivity<ISendDomainVerifiedEmailArguments>
    {
    }
}
