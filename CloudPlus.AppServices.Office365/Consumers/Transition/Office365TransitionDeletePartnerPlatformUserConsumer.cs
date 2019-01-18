using System.Threading.Tasks;
using MassTransit;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Services.Office365.User;

namespace CloudPlus.AppServices.Office365.Consumers.Transition
{
    public class Office365TransitionDeletePartnerPlatformUserConsumer : IOffice365TransitionDeletePartnerPlatformUserConsumer
    {
        private readonly IOffice365UserService _office365UserService;

        public Office365TransitionDeletePartnerPlatformUserConsumer(IOffice365UserService office365UserService)
        {
            _office365UserService = office365UserService;
        }

        public async Task Consume(ConsumeContext<IOffice365TransitionDeletePartnerPlatformUserCommand> context)
        {
            var arguments = context.Message;

            await _office365UserService.DeleteOffice365UserAsync(arguments.Office365UserId, arguments.Office365CustomerId);
        }
    }
}
