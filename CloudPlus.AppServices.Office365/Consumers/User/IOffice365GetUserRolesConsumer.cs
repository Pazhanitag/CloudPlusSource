using CloudPlus.QueueModels.Office365.User;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public interface IOffice365GetUserRolesConsumer : IConsumer<IOffice365GetUserRolesRequest>
    {
    }
}
