using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public interface IOffice365UserAssignLicenseConsumer : IConsumer<IOffice365UserAssignLicenseCommand>
    {
    }
}
