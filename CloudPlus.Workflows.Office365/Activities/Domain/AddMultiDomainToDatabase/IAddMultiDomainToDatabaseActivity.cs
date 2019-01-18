using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase
{
    public interface IAddMultiDomainToDatabaseActivity : Activity<IAddMultiDomainToDatabaseArguments, IAddMultiDomainToDatabaseLog>
    {
    }
}
