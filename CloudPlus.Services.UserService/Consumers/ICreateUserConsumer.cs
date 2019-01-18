using CloudPlus.QueueModels.Users.Commands;
using MassTransit;

namespace CloudPlus.AppServices.User.Consumers
{
    public interface ICreateUserConsumer : IConsumer<ICreateUserCommand>
    {
    }
}