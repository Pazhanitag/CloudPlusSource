using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.CreateActiveDirectoryCompany
{
    public interface ICreateActiveDirectoryComapnyActivity : Activity<ICreateActiveDirectoryCompanyArguments, ICreateActiveDirectoryCompanyLog>
    {
    }
}