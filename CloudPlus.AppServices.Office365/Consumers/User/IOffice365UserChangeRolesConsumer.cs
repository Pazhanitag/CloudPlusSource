using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public interface IOffice365UserChangeRolesConsumer : IConsumer<IOffice365UserChangeRolesCommand>
    {
    }
}
