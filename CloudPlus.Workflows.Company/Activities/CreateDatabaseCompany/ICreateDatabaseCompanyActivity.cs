using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.CreateDatabaseCompany
{
    public interface ICreateDatabaseCompanyActivity : Activity<ICreateDatabaseCompanyArguments, ICreateDatabaseCompanyLog>
    {
    }
}