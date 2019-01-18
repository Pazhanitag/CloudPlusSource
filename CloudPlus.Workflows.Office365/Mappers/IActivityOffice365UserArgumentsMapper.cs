using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365UserArgumentsMapper
    {
        dynamic MapCreatePartnerPlatformUserArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapCreateOffice365DatabaseUserArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapPartnerPlatformOffice365CustomerSubscriptionArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapUpdateDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapAssignLicenseOffice365PartnerPlatformUserArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapAssignLicenseOffice365DatabaseUserArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapAssignUserRolesArguments(IOffice365UserAssignLicenseCommand src);
        dynamic MapSendOffice365UserSetupEmailArguments(IOffice365UserAssignLicenseCommand src);

        dynamic MapDeletePartnerPlatformUserArguments(IOffice365UserRemoveLicenseCommand src);
        dynamic MapSoftDeleteDatabaseUserArguments(IOffice365UserRemoveLicenseCommand src);
        dynamic MapDecreasePartnerPlatformCustomerSubscriptionArguments(IOffice365UserRemoveLicenseCommand src);
        dynamic MapDecreaseDatabaseCustomerSubscriptionArguments(IOffice365UserRemoveLicenseCommand src);
    }
}
