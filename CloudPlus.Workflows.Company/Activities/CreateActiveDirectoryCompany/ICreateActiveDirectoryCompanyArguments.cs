using CloudPlus.Models.Company;

namespace CloudPlus.Workflows.Company.Activities.CreateActiveDirectoryCompany
{
    public interface ICreateActiveDirectoryCompanyArguments
    {
        CompanyModel Company { get; set; }
    }
}
