using System.Collections.Generic;
using CloudPlus.QueueModels.Office365.User.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public interface IOffice365CreateUserConsumer : IConsumer<IOffice365UserCreateCommand>
    {

    }
}