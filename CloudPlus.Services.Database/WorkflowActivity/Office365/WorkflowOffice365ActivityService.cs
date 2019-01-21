using System.Collections.Generic;
using System.Linq;
using CloudPlus.Logging;
using CloudPlus.Models.Enums;
using CloudPlus.Models.WorkflowActivity;

namespace CloudPlus.Services.Database.WorkflowActivity.Office365
{
    public class WorkflowOffice365ActivityService : IWorkflowOffice365ActivityService
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public WorkflowOffice365ActivityService(IWorkflowActivityService workflowActivity)
        {
            _workflowActivityService = workflowActivity;
        }

        public bool IsOffice365ProvisioningInProgress(int companyId)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.companyId", companyId },
                { "Data.workflowActivityType", WorkflowActivityType.CreateOffice365Customer.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365DomainVerificationInProgress(string domain)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.domainName", domain },
                { "Data.workflowActivityType", WorkflowActivityType.VerifyAndFederateOffice365Domain.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365AddingAdditionalDomainInProgress(string domain)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.domain", domain },
                { "Data.workflowActivityType", WorkflowActivityType.Office365AddAdditionalDomain.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceAssignmentInProgress(string userPrincipalName)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserAssignLicense.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceChangingInProgress(string userPrincipalName)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserChangeLicense.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceRemovalInProgress(string userPrincipalName)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserRemoveLicense.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceRestoreInProgress(string userPrincipalName)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserRestore.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserRolesChangingInProgress(string userPrincipalName)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserChangeRoles.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365ManageSubscriptionInProgress(string userPrincipalName)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365ManageSubscription.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            var manageSubscriptionActivities = _workflowActivityService
                .Get("Data.workflowActivityType", WorkflowActivityType.Office365ManageSubscription.ToString()).ToList();

            var allActivities = manageSubscriptionActivities.Where(a => a.UniqueId == activities.Last().UniqueId).ToList();

            var allActivitiesPrincipal = new List<WorkflowActivityDto>();

            foreach (var activity in allActivities)
            {
                if (activity.Context.Data.Values.Any(p => p.ToString().ToLower() == userPrincipalName.ToLower()))
                    allActivitiesPrincipal.Add(activity);
                else
                    if (activity.Context.Data.Values.Any(p => p.ToString().ToLower().Contains(userPrincipalName.ToLower())))
                        allActivitiesPrincipal.Add(activity);
            }

            return CheckStatusV2(allActivitiesPrincipal);
        }


        //Start individual license status checking 

        public bool IsOffice365UserLicenceAssignmentForLicenseInProgress(string userPrincipalName,string License)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserAssignLicense.ToString() },
                { "Data.cloudPlusProductIdentifier",License }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceChangingForLicenseInProgress(string userPrincipalName,string License)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserChangeLicense.ToString() },
                 { "Data.cloudPlusProductIdentifier",License }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceRemovalForLicenseInProgress(string userPrincipalName,string License)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserRemoveLicense.ToString() },
                 { "Data.cloudPlusProductIdentifier",License }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365UserLicenceRestoreForLicenseInProgress(string userPrincipalName, string License)
        {
            var manageSubscription = IsOffice365ManageSubscriptionInProgress(userPrincipalName);

            if (manageSubscription) return true;

            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365UserRestore.ToString() },
                 { "Data.cloudPlusProductIdentifier",License }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365ManageSubscriptionForLicenseInProgress(string userPrincipalName, string License)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365ManageSubscription.ToString() },
                 { "Data.cloudPlusProductIdentifier",License }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            var manageSubscriptionActivities = _workflowActivityService
                .Get("Data.workflowActivityType", WorkflowActivityType.Office365ManageSubscription.ToString()).ToList();

            var allActivities = manageSubscriptionActivities.Where(a => a.UniqueId == activities.Last().UniqueId).ToList();

            var allActivitiesPrincipal = new List<WorkflowActivityDto>();

            foreach (var activity in allActivities)
            {
                if (activity.Context.Data.Values.Any(p => p.ToString().ToLower() == userPrincipalName.ToLower()))
                    allActivitiesPrincipal.Add(activity);
                else
                    if (activity.Context.Data.Values.Any(p => p.ToString().ToLower().Contains(userPrincipalName.ToLower())))
                    allActivitiesPrincipal.Add(activity);
            }

            return CheckStatusV2(allActivitiesPrincipal);
        }

        //End For Individual License
        public bool IsOffice365TransitionInProgress(int companyId)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.companyId", companyId.ToString() },
                { "Data.workflowActivityType", WorkflowActivityType.Office365Transition.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }
        
        public bool IsOffice365TransitionStarted(int companyId)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.companyId", companyId.ToString() },
                { "Data.workflowActivityType", WorkflowActivityType.Office365Transition.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            return activities.Any();
        }

        public bool IsOffice365TransitionDeletePartnerPlatformUserInProgress(string userPrincipalName)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365TransitionDeletePartnerPlatformUser.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public bool IsOffice365TransitionUserAndLicensesInProgress(string userPrincipalName)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365TransitionUserAndLicenses.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return false;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return CheckStatusV2(activities);
        }

        public WorkflowActivityStatus Office365TransitionDeletePartnerPlatformUserStatus(string userPrincipalName)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365TransitionDeletePartnerPlatformUser.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return WorkflowActivityStatus.None;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return GetStatus(activities);
        }

        public WorkflowActivityStatus Office365TransitionUserAndLicensesStatus(string userPrincipalName)
        {
            var activities = _workflowActivityService.Get(new Dictionary<string, object>
            {
                { "Data.userPrincipalName", userPrincipalName },
                { "Data.workflowActivityType", WorkflowActivityType.Office365TransitionUserAndLicenses.ToString() }
            }).OrderBy(o => o.Context.Timestamp).ToList();

            if (!activities.Any()) return WorkflowActivityStatus.None;

            if (activities.Count > 1)
                activities = _workflowActivityService
                    .Get("TrackingNumber", activities.LastOrDefault()?.Context.TrackingNumber).ToList();

            return GetStatus(activities);
        }

        private bool CheckStatusV2(IReadOnlyCollection<WorkflowActivityDto> activities)
        {
            var orderedActivities = activities.OrderBy(o => o.Context.Timestamp).ToList();

            var routingSlipFaulted = orderedActivities.FirstOrDefault(f =>
                f.Context.WorkflowActivityStatus == WorkflowActivityStatus.RoutingSlipFaulted);

            if (routingSlipFaulted != null) return false;

            var routingSlipCompleted = orderedActivities.FirstOrDefault(f =>
                f.Context.WorkflowActivityStatus == WorkflowActivityStatus.RoutingSlipCompleted);

            return routingSlipCompleted == null;
        }

        private WorkflowActivityStatus GetStatus(IReadOnlyCollection<WorkflowActivityDto> activities)
        {
            var orderedActivities = activities.OrderBy(o => o.Context.Timestamp).ToList();

            var routingSlipFaulted = orderedActivities.FirstOrDefault(f =>
                f.Context.WorkflowActivityStatus == WorkflowActivityStatus.RoutingSlipFaulted);

            if (routingSlipFaulted != null) return WorkflowActivityStatus.RoutingSlipFaulted;

            var routingSlipCompleted = orderedActivities.FirstOrDefault(f =>
                f.Context.WorkflowActivityStatus == WorkflowActivityStatus.RoutingSlipCompleted);

            return routingSlipCompleted != null ? WorkflowActivityStatus.ActivityCompleted : WorkflowActivityStatus.RoutingSlipStart;
        }
    }
}
