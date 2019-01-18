using CloudPlus.Enums.User;
using CloudPlus.Models.Company;

namespace CloudPlus.Workflows.Company.Activities.CompanyCreated
{
    public interface ICompanyCreatedArguments
    {
        CompanyModel Company { get; set; }
        int UserId { get; set; }
        string Email { get; set; }
        string AlternativeEmail { get; set; }
        string Password { get; set; }
        PasswordSetupMethod PasswordSetupMethod { get; set; }
        bool SendPlainPasswordViaEmail { get; set; }
        string PasswordSetupEmail { get; set; }
        bool SendWelcomeLetters { get; set; }
    }
}
