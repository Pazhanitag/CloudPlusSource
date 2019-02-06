using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.User;
using CloudPlus.Constants;
using CloudPlus.Services.Identity.Permission;
using CloudPlus.Services.Identity.User;
using CloudPlus.Services.Database.Provisions;
using CloudPlus.Api.ViewModels.Request.Office365;

namespace CloudPlus.Api.Controllers.UsersAndRoles
{
    [System.Web.Http.Authorize]
    [RoutePrefix("api/office365groups")]
    public class Office365GroupsController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        public Office365GroupsController(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "CreateSecurityGroup")]
        public async Task<IHttpActionResult> CreateSecurityGroup([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createSecurityGroupQueue = UserServiceConstants.QueueCreateSecurityGroup;

            await _messageBroker.GetSendEndpoint(createSecurityGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "CreateDistributionGroup")]
        public async Task<IHttpActionResult> CreateDistributionGroup([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createDistributionGroupQueue = UserServiceConstants.QueueCreateDistributionGroup;

            await _messageBroker.GetSendEndpoint(createDistributionGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "CreateOffice365Group")]
        public async Task<IHttpActionResult> CreateOffice365Group([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createSecurityGroupQueue = UserServiceConstants.QueueCreateSecurityGroup;

            await _messageBroker.GetSendEndpoint(createSecurityGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "RemoveSecurityGroup")]
        public async Task<IHttpActionResult> RemoveSecurityGroup([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createSecurityGroupQueue = UserServiceConstants.QueueCreateSecurityGroup;

            await _messageBroker.GetSendEndpoint(createSecurityGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }


        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "RemoveDistributionGroup")]
        public async Task<IHttpActionResult> RemoveDistributionGroup([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createSecurityGroupQueue = UserServiceConstants.QueueCreateSecurityGroup;

            await _messageBroker.GetSendEndpoint(createSecurityGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }


        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "RemoveOffice365Group")]
        public async Task<IHttpActionResult> RemoveOffice365Group([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createSecurityGroupQueue = UserServiceConstants.QueueCreateSecurityGroup;

            await _messageBroker.GetSendEndpoint(createSecurityGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }


        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.AddUsers)]
        [Route(Name = "GetAllGroups")]
        public async Task<IHttpActionResult> GetAllGroups([FromBody]Office365SecurityGroupViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createSecurityGroupQueue = UserServiceConstants.QueueCreateSecurityGroup;

            await _messageBroker.GetSendEndpoint(createSecurityGroupQueue)
                .Send<ICreateSecurityGroupCommand>(
                    model.ToCreateSecurityGroupCommand()
                );

            return Ok();
        }


    }
}