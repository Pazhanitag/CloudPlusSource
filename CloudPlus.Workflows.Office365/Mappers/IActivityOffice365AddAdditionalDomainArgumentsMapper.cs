using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365AddAdditionalDomainArgumentsMapper
    {
        dynamic MapAddCustomerDomainPartnerPortalArguments(IOffice365AddAdditionalDomainCommand command);
        dynamic MapAddCustomerDomainToDatabaseArguments(IOffice365AddAdditionalDomainCommand command);
        dynamic MapGetCustomerTxtRecords(IOffice365AddAdditionalDomainCommand command);
        dynamic MapSendCustomerTxtRecords(IOffice365AddAdditionalDomainCommand command);
    }
}
