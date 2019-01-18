using MassTransit.EntityFrameworkIntegration;
using MassTransit.Saga;

namespace CloudPlus.Infrastructure.ServiceBus.DbContext
{
    public class WorkflowSagaClassMapping<T> : SagaClassMapping<T> where T : class, ISaga
    {
    }
}