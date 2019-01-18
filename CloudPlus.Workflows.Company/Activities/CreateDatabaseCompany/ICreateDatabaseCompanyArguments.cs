using CloudPlus.Enums.User;
using CloudPlus.Models.Company;
using CloudPlus.Models.Identity;

namespace CloudPlus.Workflows.Company.Activities.CreateDatabaseCompany
{
    public interface ICreateDatabaseCompanyArguments
    {
        CompanyModel Company { get; set; }
        UserModel User { get; set; }
        int CompanyOu { get; set; }
        string Password { get; set; }
        PasswordSetupMethod PasswordSetupMethod { get; set; }
        string PasswordSetupEmail { get; set; }
        UserStatus UserStatus { get; set; }
    }
}
