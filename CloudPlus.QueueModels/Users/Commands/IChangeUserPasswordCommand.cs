using CloudPlus.Enums.User;

namespace CloudPlus.QueueModels.Users.Commands
{
    public interface IChangeUserPasswordCommand : IQueueModel
    {
        int UserId { get; set; }
        string Password { get; set; }
        PasswordSetupMethod PasswordSetupMethod { get; set; }
        bool SendPlainPasswordViaEmail { get; set; }
        string PasswordSetupEmail { get; set; }
    }
}