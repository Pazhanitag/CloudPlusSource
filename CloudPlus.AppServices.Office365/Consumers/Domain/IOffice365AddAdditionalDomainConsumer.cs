using MassTransit;
using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.Domain
{
    public interface IOffice365AddAdditionalDomainConsumer : IConsumer<IOffice365AddAdditionalDomainCommand>
    {
    }
}
