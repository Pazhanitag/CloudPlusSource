using MassTransit.EntityFrameworkIntegration;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.Saga;

namespace CloudPlus.Infrastructure.ServiceBus.DbContext
{
    public class WorkflowRepository<TSaga> : IWorkflowRepository<TSaga> where TSaga : class, ISaga
    {
        private readonly SagaDbContextFactory _sagaDbContextFactory = () => new WorkflowDbContext<TSaga>();

        public ISagaRepository<TSaga> Create()
        {
            return
                new EntityFrameworkSagaRepository<TSaga>(_sagaDbContextFactory)
                ;
        }
    }
}