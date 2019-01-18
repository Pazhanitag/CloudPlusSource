using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365TransitionArgumentsMapper : IActivityOffice365TransitionArgumentsMapper
    {
        public dynamic MapCreateDatabaseCustomerArguments(IOffice365TransitionCommand src)
        {
            return new
            {
                src.CompanyId,
                src.Office365CustomerId,
                src.Domains,
                WorkflowActivityType = WorkflowActivityType.Office365Transition,
                WorkflowStep = WorkflowActivityStep.Office365CreateDatabaseCustomer
            };
        }

        public dynamic MapAddMultiDomainToDatabaseArguments(IOffice365TransitionCommand src)
        {
            return new
            {
                src.CompanyId,
                src.Office365CustomerId,
                src.Domains,
                WorkflowActivityType = WorkflowActivityType.Office365Transition,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainDatabase
            };
        }

        public dynamic MapMultiPartnerPlatformCustomerSubscriptionArguments(IOffice365TransitionCommand src)
        {
            return new
            {
                src.CompanyId,
                src.Office365CustomerId,
                src.ProductItems,
                WorkflowActivityType = WorkflowActivityType.Office365Transition,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainDatabase
            };
        }

        public dynamic MapMultiDatabaseCustomerSubscriptionArguments(IOffice365TransitionCommand src)
        {
            return new
            {
                src.CompanyId,
                WorkflowActivityType = WorkflowActivityType.Office365Transition,
                WorkflowStep = WorkflowActivityStep.Office365AddCustomerDomainDatabase
            };
        }

        public dynamic MapDatabaseProvisionedStatusProvisionedArguments(IOffice365TransitionCommand src)
        {
            return new
            {
                src.CompanyId,
                src.ProductId,
                WorkflowActivityType = WorkflowActivityType.Office365Transition,
                WorkflowStep = WorkflowActivityStep.Office365DatabaseProvisionedStatusProvisioned
            };
        }

        public dynamic MapTransitionDispatchCreatingUserArguments(IOffice365TransitionCommand src)
        {
            return new
            {
                src.CompanyId,
                src.Office365CustomerId,
                src.Domains,
                src.ProductItems,
                WorkflowActivityType = WorkflowActivityType.Office365Transition,
                WorkflowStep = WorkflowActivityStep.Office365TransitionDispatchCreatingUsers
            };
        }
    }
}
