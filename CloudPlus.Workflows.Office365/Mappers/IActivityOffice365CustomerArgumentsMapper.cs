using CloudPlus.QueueModels.Office365.Customer.Commands;
using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365CustomerArgumentsMapper
    {
        dynamic MapCreatePartnerPlatformCustomerArguments(IOffice365CreateCustommerCommand command);
        dynamic MapCreateDatabaseCustomerArguments(IOffice365CreateCustommerCommand command);
        dynamic MapAddCustomerDomainPartnerPortalArguments(IOffice365CreateCustommerCommand src);
        dynamic MapAddCustomerDomainToDatabaseArguments(IOffice365CreateCustommerCommand src);
        dynamic MapGetCustomerTxtRecords(IOffice365CreateCustommerCommand src);
        dynamic MapSendCustomerTxtRecords(IOffice365CreateCustommerCommand src);
        dynamic MapGetCustomerTxtRecords(IOffice365ResendTxtRecordsCommand src);
        dynamic MapSendCustomerTxtRecords(IOffice365ResendTxtRecordsCommand src);
        dynamic MapVerifyCustomerDomain(IOffice365VerifyDomainCommand src);
        dynamic MapVerifyCustomerDomainDatabaseStatus(IOffice365VerifyDomainCommand src);
        dynamic MapCreateTempAdminUser(IOffice365VerifyDomainCommand src);
        dynamic MapFederateCustomerDomain(IOffice365VerifyDomainCommand src);
        dynamic MapFederateCustomerDomainDatabaseStatus(IOffice365VerifyDomainCommand src);
        dynamic MapDeleteTempAdminUser(IOffice365VerifyDomainCommand src);
        dynamic MapAssignUserRoles(IOffice365VerifyDomainCommand src);
    }
}
