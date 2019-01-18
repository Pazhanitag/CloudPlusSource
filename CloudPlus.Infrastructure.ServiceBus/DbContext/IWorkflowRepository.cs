using MassTransit.Saga;

namespace CloudPlus.Infrastructure.ServiceBus.DbContext
{
    public interface IWorkflowRepository<TSaga> where TSaga : class, ISaga
    {
        ISagaRepository<TSaga> Create();
    }
}