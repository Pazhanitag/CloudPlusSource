using CloudPlus.QueueModels.Office365.User.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public interface IOffice365UserMultiEditConsumer : IConsumer<IOffice365UserMultiEditCommand>
    {
    }
}
