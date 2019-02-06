using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Api.ViewModels.Response.Paging;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using CloudPlus.QueueModels.Office365.User;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.Office365.License;
using CloudPlus.Services.Database.Office365.User;

namespace CloudPlus.Api.Controllers.Office365
{
    [Authorize]
    [RoutePrefix("api/Office365Users")]
    public class Office365UserController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IOffice365DbLicenseService _office365DbLicenseService;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IDomainService _domainService;

        public Office365UserController(
            IMessageBroker messageBroker,
            IOffice365DbLicenseService office365DbLicenseService,
            IOffice365DbUserService office365DbUserService, IDomainService domainService)
        {
            _messageBroker = messageBroker;
            _office365DbLicenseService = office365DbLicenseService;
            _office365DbUserService = office365DbUserService;
            _domainService = domainService;
        }

        [HttpGet]
        [Route("GetDomainUsers/{domainName}/{page:int}/{pageSize:int}/{orderByColumn}/{order}/{searchTerm?}")]
        //[AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public async Task<IHttpActionResult> GetDomainUsers(string domainName, int page, int pageSize, string orderByColumn, string order, string searchTerm = "")
        {
            var domain = _domainService.GetDomainByName(domainName);

            if (domain.CompanyId != User.CompanyId())
                return NotFound();

            var users = await _office365DbUserService.GetUsersByDomainAsync(domainName, searchTerm, User.CompanyId());

            var pageResponse = new PagedResultViewModel<Office365DomainUserViewModel>(Request, users.Select(u => u.ToOffice365DomainUserViewModel(Url.Content("~/"))).AsQueryable(),
                page, pageSize, orderByColumn, order, "GetDomainUsers");

            return pageResponse;
        }

        [HttpGet]
        [Route("{username}/AssignedRoles", Name = "AssignedRoles")]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public async Task<IHttpActionResult> AssignedRoles(string username, int companyId)
        {
            var getUserRolesEndpoint = Office365ServiceConstants.QueueOffice365GetUserRolesUri;

            var client =
                _messageBroker.GetRequestClient<IOffice365GetUserRolesRequest, IOffice365GetUserRolesResponse>(
                    getUserRolesEndpoint, TimeSpan.FromSeconds(60));

            var response = await client.Request(new Office365GetUserRolesRequest
            {
                CompanyId = companyId,
                UserPrincipalName = username
            });

            return Ok(response);
        }

        [HttpGet]
        [Route("{username}/AssignedLicenses", Name = "AssignedLicenses")]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public async Task<IHttpActionResult> AssignedLicenses(string username)
        {
            var license = await _office365DbLicenseService.GetUserAssgnedLicenseAsync(username);
            return Ok(license);
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("AssignLicense", Name = "AssignLicense")]
        public async Task<IHttpActionResult> AssignLicense([FromBody]Office365UserAssignLicenseViewModel model)
        {
            var office365UserAssignLicense = Office365ServiceConstants.QueueManageSubscriptionsAndLicences;

            if (string.IsNullOrWhiteSpace(model.CloudPlusProductIdentifier))
            {
                await _messageBroker.GetSendEndpoint(Office365ServiceConstants.QueueOffice365CreateUser)
                    .Send<IOffice365UserCreateCommand>(new
                    {
                        model.CompanyId,
                        model.UserPrincipalName,
                        UsageLocation = "US",
                        model.UserRoles,
                        Password = string.IsNullOrEmpty(model.Password) ? null : model.Password
                    });
            }
            else
            {
                await _messageBroker.GetSendEndpoint(office365UserAssignLicense)
                    .Send<IManageSubscriptionsAndLicencesCommand>(model.ToManageSubscriptionsAndLicencesCommand());
            }
            
            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("ChangeLicense", Name = "ChangeLicense")]
        public async Task<IHttpActionResult> ChangeLicense([FromBody]Office365UserChangeLicenseViewModel model)
        {
            var office365UserChangeLicense = Office365ServiceConstants.QueueManageSubscriptionsAndLicences;

            if (model.AssignCloudPlusProductIdentifier == model.RemoveCloudPlusProductIdentifier)
            {
                await _messageBroker.GetSendEndpoint(office365UserChangeLicense)
                    .Send<IOffice365UserChangeRolesCommand>(model.ToOffice365UserChangeRolesCommand());
            }
            else
            {
                await _messageBroker.GetSendEndpoint(office365UserChangeLicense)
                    .Send<IManageSubscriptionsAndLicencesCommand>(model.ToManageSubscriptionsAndLicencesCommand());
            }

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("RemoveLicense", Name = "RemoveLicense")]
        public async Task<IHttpActionResult> RemoveLicense([FromBody]Office365UserRemoveLicenseViewModel model)
        {
            var office365UserRemoveLicense = Office365ServiceConstants.QueueManageSubscriptionsAndLicences;

            await _messageBroker.GetSendEndpoint(office365UserRemoveLicense)
                .Send<IManageSubscriptionsAndLicencesCommand>(model.ToManageSubscriptionsAndLicencesCommand());

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("Restore", Name = "Restore")]
        public async Task<IHttpActionResult> Restore([FromBody]Office365UserRestoreViewModel model)
        {
            var office365UserRestore = Office365ServiceConstants.QueueManageSubscriptionsAndLicences;

            await _messageBroker.GetSendEndpoint(office365UserRestore)
                .Send<IManageSubscriptionsAndLicencesCommand>(model.ToManageSubscriptionsAndLicencesCommand());

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("MultiEdit", Name = "MultiEdit")]
        public async Task<IHttpActionResult> MultiEdit([FromBody]Office365UserMultiAddViewModel model)
        {
            var office365UserMultiEdit = Office365ServiceConstants.QueueOffice3655UserMultiEdit;

            await _messageBroker.GetSendEndpoint(office365UserMultiEdit)
                .Send<IManageSubscriptionsAndLicencesCommand>(model.ToManageSubscriptionsAndLicencesCommand());

            return Ok();
        }


        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("EditUser", Name = "EditUser")]
        public async Task<IHttpActionResult> EditUser([FromBody]Office365UserEditViewModel model)
        {
            var office365EditUser = Office365ServiceConstants.QueueManageSubscriptionsAndLicences;

            await _messageBroker.GetSendEndpoint(office365EditUser)
                .Send<IManageSubscriptionsAndLicencesCommand>(model.ToManageSubscriptionsAndLicencesCommand());

            return Ok();
        }
    }
}
