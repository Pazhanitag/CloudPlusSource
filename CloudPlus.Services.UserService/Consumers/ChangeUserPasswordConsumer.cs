using System.Threading.Tasks;
using CloudPlus.Constants;
using MassTransit;
using CloudPlus.Enums.Notification;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.ActiveDirectory;
using CloudPlus.Services.Identity.Password;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.AppServices.User.Consumers
{
    public class ChangeUserPasswordConsumer : IChangeUserPasswordConsumer
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IActiveDirectoryService _activeDirectoryService;

        public ChangeUserPasswordConsumer(
            IUserService userService,
            IPasswordService passwordService,
            IActiveDirectoryService activeDirectoryService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _activeDirectoryService = activeDirectoryService;
        }

        public async Task Consume(ConsumeContext<IChangeUserPasswordCommand> context)
        {
            var changePasswordArguments = context.Message;
            var user = await _userService.GetUserAsync(changePasswordArguments.UserId);
            var sendEmailEndpoint = context.GetSendEndpoint(NotificationServiceConstants.SendEmailNotificationUri);

            if (changePasswordArguments.PasswordSetupMethod == Enums.User.PasswordSetupMethod.GeneratePasswordViaLink)
            {
                var tempResetLink = await _passwordService.GetPasswordResetLink(user.Id, user.Email);

                await sendEmailEndpoint.Result.Send<ISendEmailCommand>(
                    new
                    {
                        UserName = user.Email,
                        TempResetLink = tempResetLink,
                        To = changePasswordArguments.PasswordSetupEmail,
                        EmailTemplateType = EmailTemplateType.ChangePassword,
                    });
            }
            else
            {
                _activeDirectoryService.UpdateUserPassword(user.Email, changePasswordArguments.Password);

                if (changePasswordArguments.SendPlainPasswordViaEmail)
                {
                    await sendEmailEndpoint.Result.Send<ISendEmailCommand>(
                        new
                        {
                            UserName = user.Email,
                            To = changePasswordArguments.PasswordSetupEmail,
                            EmailTemplateType = EmailTemplateType.PasswordChangedSendPlainPasswordViaEmail,
                            changePasswordArguments.Password
                        });
                }
                else
                {
                    await sendEmailEndpoint.Result.Send<ISendEmailCommand>(
                        new
                        {
                            UserName = user.Email,
                            To = user.Email,
                            EmailTemplateType = EmailTemplateType.PasswordChanged,
                        });
                }
            }
        }
    }
}
