using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365AddAdditionalDomainArgumentsMapper : IActivityOffice365AddAdditionalDomainArgumentsMapper
    {
        public dynamic MapAddCustomerDomainPartnerPortalArguments(IOffice365AddAdditionalDomainCommand command)
        {
            var dest = new
            {
                command.Office365CustomerId,
                command.Domain,
                WorkflowActivityType = WorkflowActivityType.Office365AddAdditionalDomain,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainPartnerPlatform
            };

            return dest;
        }

        public dynamic MapAddCustomerDomainToDatabaseArguments(IOffice365AddAdditionalDomainCommand command)
        {
            var dest = new
            {
                command.Office365CustomerId,
                command.Domain,
                WorkflowActivityType = WorkflowActivityType.Office365AddAdditionalDomain,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainDatabase
            };

            return dest;
        }

        public dynamic MapGetCustomerTxtRecords(IOffice365AddAdditionalDomainCommand command)
        {
            var dest = new
            {
                command.Office365CustomerId,
                command.Domain,
                WorkflowActivityType = WorkflowActivityType.Office365AddAdditionalDomain,
                WorkflowStep = WorkflowActivityStep.Office365GetCustomerTxtRecords
            };

            return dest;
        }

        public dynamic MapSendCustomerTxtRecords(IOffice365AddAdditionalDomainCommand command)
        {
            var dest = new
            {
                command.CompanyId,
                command.Email,
                command.Domain,
                WorkflowActivityType = WorkflowActivityType.Office365AddAdditionalDomain,
                WorkflowStep = WorkflowActivityStep.Office365SendCustomerTxtRecords
            };

            return dest;
        }
    }
}
