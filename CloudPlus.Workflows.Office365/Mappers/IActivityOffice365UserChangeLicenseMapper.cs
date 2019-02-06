using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365UserChangeLicenseMapper
    {
        dynamic MapDecreasePartnerPlatformCustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapDecreaseDatabaseCustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapRemoveLicensePartnerPortalUserArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapRemoveLicenseDatabaseUserArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapPartnerPlatformOffice365CustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapGetUserRolesArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapRemoveUserRolesArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapAssignUserRolesArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapAssignLicenseOffice365PartnerPlatformUserArguments(IOffice365UserChangeLicenseCommand src);
        dynamic MapAssignLicenseOffice365DatabaseUserArguments(IOffice365UserChangeLicenseCommand src);
    }
}
