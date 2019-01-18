using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.Enums.Notification;
using MassTransit;
using CloudPlus.Models.Enums;
using CloudPlus.Models.Office365.Transition;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.ProductItem;
using CloudPlus.Services.Database.WorkflowActivity.Office365;
using CloudPlus.Settings;

namespace CloudPlus.AppServices.Office365.Consumers.Transition
{
    public class Office365TransitionReportConsumer : IOffice365TransitionReportConsumer
    {
        private readonly IWorkflowOffice365ActivityService _workflowService;
        private readonly IProductItemService _productItemService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365ServiceSettings _office365ServiceSettings;

        public Office365TransitionReportConsumer(
            IWorkflowOffice365ActivityService workflowService,
            IProductItemService productItemService,
            IOffice365DbCustomerService office365DbCustomerServic,
            IOffice365ServiceSettings office365ServiceSettings)
        {
            _workflowService = workflowService;
            _productItemService = productItemService;
            _office365DbCustomerService = office365DbCustomerServic;
            _office365ServiceSettings = office365ServiceSettings;
        }

        public async Task Consume(ConsumeContext<IOffice365TransitionReportCommand> context)
        {
            var message = context.Message;

            var transitionStarted = _workflowService.IsOffice365TransitionStarted(message.CompanyId);
            var transitionInProgress = _workflowService.IsOffice365TransitionInProgress(message.CompanyId);

            if (!transitionStarted || transitionInProgress)
            {
#pragma warning disable 4014
                context.Redeliver(TimeSpan.FromSeconds(30));
#pragma warning restore 4014
                return;
            }

            var report = new Office365TransitionReportModel();
            var deletedUsers = new List<string>();
            var adminUsers = new List<string>();
            var licenseUsers = new List<string>();

            foreach (var item in message.ProductItems)
            {
                if (item.Delete)
                {
                    transitionInProgress =
                        _workflowService.IsOffice365TransitionDeletePartnerPlatformUserInProgress(
                            item.UserPrincipalName);

                    if (transitionInProgress)
                    {
#pragma warning disable 4014
                        context.Redeliver(TimeSpan.FromSeconds(30));
#pragma warning restore 4014
                        return;
                    }

                    deletedUsers.Add(item.UserPrincipalName);
                }
                else if (item.KeepLicenses)
                {
                    report.KeepLicensesUsers.Add(item.UserPrincipalName);
                }
                else
                {
                    transitionInProgress =
                        _workflowService.IsOffice365TransitionUserAndLicensesInProgress(item.UserPrincipalName);

                    if (transitionInProgress)
                    {
#pragma warning disable 4014
                        context.Redeliver(TimeSpan.FromSeconds(30));
#pragma warning restore 4014
                        return;
                    }

                    if (item.Admin)
                        adminUsers.Add(item.UserPrincipalName);
                    else
                        licenseUsers.Add(item.UserPrincipalName);
                }
            }

            foreach (var user in deletedUsers)
            {
                var status = _workflowService.Office365TransitionDeletePartnerPlatformUserStatus(user);

                if (status == WorkflowActivityStatus.ActivityCompleted)
                    report.DeletedUsersSucceed.Add(user);
                else
                    report.DeletedUsersFailed.Add(user);
            }

            foreach (var user in adminUsers)
            {
                var status = _workflowService.Office365TransitionUserAndLicensesStatus(user);

                if (status == WorkflowActivityStatus.ActivityCompleted)
                    report.AdminUsersSucceed.Add(user);
                else
                    report.AdminUsersFailed.Add(user);
            }

            foreach (var user in licenseUsers)
            {
                var status = _workflowService.Office365TransitionUserAndLicensesStatus(user);

                if (status == WorkflowActivityStatus.ActivityCompleted)
                    report.LicenseUsersSucceed.Add(user);
                else
                    report.LicenseUsersFailed.Add(user);
            }

            var office365Customer =
                await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(message.CompanyId);
            report.Domains = office365Customer.Domains.Select(d => d.Name).ToList();
            var productItems = _productItemService.GetProductItems().ToList();

            foreach (var subscription in office365Customer.Office365Subscriptions)
            {
                var productItem = productItems.FirstOrDefault(i =>
                    i.Identifier == subscription.Office365Offer.CloudPlusProductIdentifier);

                report.Subscriptions.Add(new Office365TransitionSubscriptionReportModel
                {
                    Name = productItem?.Name,
                    Quantity = subscription.Quantity
                });
            }

            var recipients = new List<string> { _office365ServiceSettings.CloudPlusEngineeringEmail };

            await context.Send<ISendEmailCommand>(NotificationServiceConstants.SendEmailNotificationUri, new
            {
                message.CompanyId,
                To = _office365ServiceSettings.CloudPlusSupportEmail,
                Recipients = recipients,
                EmailTemplateType = EmailTemplateType.Office365TransitionReport,
                Report = report
            });

        }
    }
}
