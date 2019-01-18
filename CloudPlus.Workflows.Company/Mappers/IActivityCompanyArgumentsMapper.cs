using CloudPlus.QueueModels.Companies.Commands;

namespace CloudPlus.Workflows.Company.Mappers
{
    public interface IActivityCompanyArgumentsMapper
    {
        dynamic MapActiveDirectoryCompanyArguments(ICreateCompanyCommand src);
        dynamic MapDatabaseCompanyArguments(ICreateCompanyCommand src);
        dynamic MapDatabaseCompanyArguments(IUpdateCompanyCommand src);
        dynamic MapCreatedComapnySendEmailArguments(ICreateCompanyCommand src);
        dynamic MapAssignCatalogArguments(ICreateCompanyCommand src);
    }
}
