using CloudPlus.QueueModels.Office365.Domain.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Domain
{
    public interface IOffice365VerifyCustomerDomainConsumer : IConsumer<IOffice365VerifyDomainCommand>
    {
    }
}
