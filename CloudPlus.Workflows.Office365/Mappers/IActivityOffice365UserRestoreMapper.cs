using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365UserRestoreMapper
    {
        dynamic MapRestorePartnerPlatformUserArguments(IOffice365UserRestoreCommand command);
        dynamic MapActivateSoftDeletedDatabaseUserArguments(IOffice365UserRestoreCommand command);
        dynamic MapPartnerPlatformOffice365CustomerSubscriptionArguments(IOffice365UserRestoreCommand command);
        dynamic MapDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserRestoreCommand command);
    }
}
