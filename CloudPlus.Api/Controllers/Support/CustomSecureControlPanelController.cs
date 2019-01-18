using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Company;
using CloudPlus.Constants;
using CloudPlus.Enums.Notification;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Models.Support;
using CloudPlus.QueueModels.Companies.Commands;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.Services.Database.Provisions;
using CloudPlus.Services.Database.Support;
using CloudPlus.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CloudPlus.Api.Controllers.Support
{
    [Authorize]
    [RoutePrefix("api/CustomSecureControlPanel")]
    public class CustomSecureControlPanelController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ICloudPlusApiSettings _cloudPlusApiSettings;
        private readonly ICustomSecureControlPanelService _customSecureControlPanelService;

        public CustomSecureControlPanelController(
            IMessageBroker messageBroker,
            ICustomSecureControlPanelService customSecureControlPanelService,
            ICloudPlusApiSettings cloudPlusApiSettings)
        {
            _messageBroker = messageBroker;
            _customSecureControlPanelService = customSecureControlPanelService;
            _cloudPlusApiSettings = cloudPlusApiSettings;
        }
        [HttpPut]
        [AuthorizeAccess(PermissionsConstants.EditAccounts, PermissionsConstants.EditUsers)]
        //[ValidateModel]
        [Route(Name = "AddCustomSecureControlPanel")]
        public async Task<IHttpActionResult> AddCustomSecureControlPanel(CustomSecureControlPanelModel model)
        {
            var CompanyEntity = await _customSecureControlPanelService.CreateCustommeSecureControlPanel(model);
            var UpdateCompany = new UpdateCompanyViewModel()
            {
                Id = CompanyEntity.Id,
                ParentId = CompanyEntity.ParentId,
                Name = CompanyEntity.Name,
                Website = CompanyEntity.Website,
                LogoBase64 = CompanyEntity.LogoUrl,
                SupportSiteUrl = CompanyEntity.SupportSiteUrl,
                ControlPanelSiteUrl = CompanyEntity.ControlPanelSiteUrl,
                Email = CompanyEntity.Email,
                PhoneNumber = CompanyEntity.PhoneNumber,
                StreetAddress = CompanyEntity.StreetAddress,
                City = CompanyEntity.City,
                ZipCode = CompanyEntity.ZipCode,
                State = CompanyEntity.State,
                Country = CompanyEntity.Country,
                BrandColorPrimary = CompanyEntity.BrandColorPrimary,
                BrandColorSecondary = CompanyEntity.BrandColorSecondary,
                BrandColorText = CompanyEntity.BrandColorText,
                Domains = CompanyEntity.Domains.AsEnumerable().Select(x => new CreateDomainViewModel
                {
                    Name = x.Name,
                    IsPrimary = x.IsPrimary
                }).ToList()
            };
            var updateCompanyQueue = CompanyServiceConstants.QueueUpdateCompany;

            await _messageBroker.GetSendEndpoint(updateCompanyQueue)
                .Send<IUpdateCompanyCommand>(
                    UpdateCompany.ToUpdateCompanyCommand()
                );
            var sendEmailQueue = NotificationServiceConstants.QueueSendEmailNotification;
            await _messageBroker.GetSendEndpoint(sendEmailQueue).Send<ISendEmailCommand>(
               new
               {
                   CustomSecureCompanyId = model.Id,
                   CompanyId = model.Id,
                   To = _cloudPlusApiSettings.CloudPlusSupportGroupEmail,
                   EmailTemplateType = EmailTemplateType.CustomSecureControlPanelActivation,
               });

            return Ok();
        }
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.EditAccounts, PermissionsConstants.EditUsers)]
        [Route(Name = "CustomControlPanelProvisioningStatus")]
        public async Task<int> CustomControlPanelProvisioningStatus(int companyId)
        {
            return (await _customSecureControlPanelService.GetProvisioningStatus(companyId));
        }
        #region GetSupportProduct
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.EditAccounts, PermissionsConstants.EditUsers)]
        [Route("GetSupportProduct", Name = "GetSupportProduct")]
        public async Task<IHttpActionResult> GetSupportProduct(int companyId)
        {
            return Ok(await _customSecureControlPanelService.GetSupportProducts(companyId));
        }
        #endregion
    }
}