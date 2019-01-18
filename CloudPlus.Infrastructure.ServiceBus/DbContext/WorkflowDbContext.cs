using MassTransit.EntityFrameworkIntegration;
using MassTransit.Saga;

namespace CloudPlus.Infrastructure.ServiceBus.DbContext
{
    public class WorkflowDbContext<T> : SagaDbContext<T, WorkflowSagaClassMapping<T>> where T : class, ISaga
    {
        public WorkflowDbContext() : base("name=CloudPlusDb")
        {
        }
    }
}