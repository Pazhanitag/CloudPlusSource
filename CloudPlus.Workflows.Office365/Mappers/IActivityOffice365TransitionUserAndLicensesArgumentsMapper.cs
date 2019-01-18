using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365TransitionUserAndLicensesArgumentsMapper
    {
        dynamic MapCreateAdUserArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapCreateIsUserArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapCreateOffice365DatabaseUserArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapGetUserRolesArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapRemoveUserRolesArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapAssignUserRolesArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapRemoveAllLicensesPartnerPortalUserArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapAssignLicenseOffice365PartnerPlatformUserArguments(IOffice365TransitionUserAndLicensesCommand src);
        dynamic MapAssignLicenseOffice365DatabaseUserArguments(IOffice365TransitionUserAndLicensesCommand src);
    }
}
