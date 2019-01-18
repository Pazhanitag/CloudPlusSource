using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.Customer.Commands;
using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365CustomerArgumentsMapper : IActivityOffice365CustomerArgumentsMapper
    {
        public dynamic MapCreatePartnerPlatformCustomerArguments(IOffice365CreateCustommerCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.FirstName,
                src.LastName,
                src.CompanyName,
                src.City,
                src.AddressLine1,
                src.AddressLine2,
                src.Country,
                src.Culture,
                src.State,
                src.PostalCode,
                src.Email,
                src.PhoneNumber,
                src.Language,
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer,
                WorkflowStep = WorkflowActivityStep.Office365CreatePartnerPlatformCustomer
            };

            return dest;
        }

        public dynamic MapCreateDatabaseCustomerArguments(IOffice365CreateCustommerCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.Domain,
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer,
                WorkflowStep = WorkflowActivityStep.Office365CreateDatabaseCustomer
            };

            return dest;
        }
        public dynamic MapAddCustomerDomainPartnerPortalArguments(IOffice365CreateCustommerCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.Domain,
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainPartnerPlatform
            };

            return dest;
        }

        public dynamic MapAddCustomerDomainToDatabaseArguments(IOffice365CreateCustommerCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.Domain,
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainDatabase
            };

            return dest;
        }

        public dynamic MapGetCustomerTxtRecords(IOffice365CreateCustommerCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.Domain,
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer,
                WorkflowStep = WorkflowActivityStep.Office365GetCustomerTxtRecords
            };

            return dest;
        }

        public dynamic MapSendCustomerTxtRecords(IOffice365CreateCustommerCommand src)
        {
            var dest = new
            {
                src.Domain,
                src.Email,
                src.CompanyId,
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer,
                WorkflowStep = WorkflowActivityStep.Office365SendCustomerTxtRecords
            };

            return dest;
        }

        public dynamic MapGetCustomerTxtRecords(IOffice365ResendTxtRecordsCommand src)
        {
            var dest = new
            {
                src.Domain,
                src.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.ResendOffice365DomainTxtRecords,
                WorkflowStep = WorkflowActivityStep.Office365GetCustomerTxtRecords
            };

            return dest;
        }

        public dynamic MapSendCustomerTxtRecords(IOffice365ResendTxtRecordsCommand src)
        {
            var dest = new
            {
                src.Email,
                src.Domain,
                src.CompanyId,
                WorkflowActivityType = WorkflowActivityType.ResendOffice365DomainTxtRecords,
                WorkflowStep = WorkflowActivityStep.Office365SendCustomerTxtRecords
            };

            return dest;
        }

        public dynamic MapVerifyCustomerDomain(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.Office365CustomerId,
                src.DomainName,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365VerifyCustomerDomain
            };

            return dest;
        }

        public dynamic MapVerifyCustomerDomainDatabaseStatus(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.DomainName,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365VerifyCustomerDomainDatabaseStatus
            };

            return dest;
        }

        public dynamic MapCreateTempAdminUser(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.Office365CustomerId,
                src.DomainName,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365CreateTempPartnerPlatformAdminUser
            };

            return dest;
        }

        public dynamic MapFederateCustomerDomain(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.Office365CustomerId,
                src.DomainName,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365FederateCustomerDomain
            };

            return dest;
        }

        public dynamic MapFederateCustomerDomainDatabaseStatus(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.DomainName,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365FederateCustomerDomain
            };

            return dest;
        }

        public dynamic MapDeleteTempAdminUser(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.Office365CustomerId,
                src.DomainName,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365HardDeletePartnerPortalUser
            };

            return dest;
        }

        public dynamic MapAssignUserRoles(IOffice365VerifyDomainCommand src)
        {
            var dest = new
            {
                src.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain,
                WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
            };

            return dest;
        }
    }
}
