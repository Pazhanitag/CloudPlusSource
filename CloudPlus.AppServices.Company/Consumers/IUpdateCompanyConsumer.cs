using CloudPlus.QueueModels.Companies.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Company.Consumers
{
    public interface IUpdateCompanyConsumer : IConsumer<IUpdateCompanyCommand>
    {
    }
}
