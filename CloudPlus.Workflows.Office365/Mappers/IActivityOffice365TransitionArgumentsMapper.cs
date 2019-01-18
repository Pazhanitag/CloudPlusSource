using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365TransitionArgumentsMapper
    {
        dynamic MapCreateDatabaseCustomerArguments(IOffice365TransitionCommand src);
        dynamic MapAddMultiDomainToDatabaseArguments(IOffice365TransitionCommand src);
        dynamic MapMultiPartnerPlatformCustomerSubscriptionArguments(IOffice365TransitionCommand src);
        dynamic MapMultiDatabaseCustomerSubscriptionArguments(IOffice365TransitionCommand src);
        dynamic MapDatabaseProvisionedStatusProvisionedArguments(IOffice365TransitionCommand src);
        dynamic MapTransitionDispatchCreatingUserArguments(IOffice365TransitionCommand src);
    }
}
