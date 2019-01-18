using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Transition.DatabaseProvisionedStatusProvisioned
{
    public interface IDatabaseProvisionedStatusProvisionedActivity
        : Activity<IDatabaseProvisionedStatusProvisionedArguments, IDatabaseProvisionedStatusProvisionedLog>
    {
    }
}
