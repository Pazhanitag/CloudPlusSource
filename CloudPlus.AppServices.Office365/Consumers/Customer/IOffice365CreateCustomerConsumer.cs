using CloudPlus.QueueModels.Office365.Customer.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Customer
{
    public interface IOffice365CreateCustomerConsumer : IConsumer<IOffice365CreateCustommerCommand>
    {
    }
}
