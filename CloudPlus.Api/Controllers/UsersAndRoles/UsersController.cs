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

namespace CloudPlus.Api.Controllers.UsersAndRoles
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IImageHelper _imageHelper;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IProvisioningService _provisioningService;
        public UsersController(
            IMessageBroker messageBroker,
            IImageHelper imageHelper,
            IUserService userService,
            IPermissionService permissionService,
            IProvisioningService provisioningService)
        {
            _messageBroker = messageBroker;
            _imageHelper = imageHelper;
            _userService = userService;
            _permissionService = permissionService;
            _provisioningService = provisioningService;
        }

        [HttpGet]
		[AuthorizeAccess (PermissionsConstants.ViewUsers)]
        [Route("{id:int}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
                return NotFound();

            if (user.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            return Ok(user.ToUserViewModel(Url.Content("~/")));
        }

        [HttpGet]
		[Route("{id:int}/permissions", Name = "GetUserPermissions")]
        public IHttpActionResult GetUserPermissions(int id)
        {
            var permissions = _permissionService.GetUserPermissions(id);

            return Ok(permissions.Select(p => p.ToPermissionViewModel()));
        }

        [HttpPost]
        [ValidateModel]
		[AuthorizeAccess(PermissionsConstants.AddUsers)]
		[Route(Name = "CreateUser")]
        public async Task<IHttpActionResult> CreateUser([FromBody]CreateUserViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            model.ProfilePicture = _imageHelper.SaveProfilePicture(model.AvatarBase64);

            var createUserQueue = UserServiceConstants.QueueCreateUser;

            await _messageBroker.GetSendEndpoint(createUserQueue)
                .Send<ICreateUserCommand>(
                    model.ToCreateUserCommand()
                );

            return Ok();
        }

        [HttpPut]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.EditUsers)]
		[Route(Name = "UpdateUser")]
        public async Task<IHttpActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (model.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            model.ProfilePicture = _imageHelper.SaveProfilePicture(model.AvatarBase64);

            var createUserQueue = UserServiceConstants.QueueUpdateUser;

            await _messageBroker.GetSendEndpoint(createUserQueue)
                .Send<IUpdateUserCommand>(
                    model.ToUpdateUserCommand()
                );

            return Ok();
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewUsers)]
		[Route("{username}/services", Name = "GetUserServices")]
        public async Task<IHttpActionResult> GetUserServices(string username, int companyId)
        {
            var service = await _provisioningService.GetServicesProvisionedToUser(username, companyId);

            return Ok(service.ToUserServiceViewModel());
        }

	    [HttpDelete]
	    [AuthorizeAccess(PermissionsConstants.DeleteUsers)]
		[Route("{userId:int}", Name = "DeleteUser")]
	    public async Task<IHttpActionResult> DeleteUser(int userId)
	    {
		    var deleteUserQueue = UserServiceConstants.QueueDeleteUser;
		    await _messageBroker.GetSendEndpoint(deleteUserQueue)
			    .Send<IDeleteUserCommand>(
				    new
				    {
					    UserId = userId,
					    CompanyId = User.CompanyId(),
						UserLoggedIn = User.UserId()
					}
			    );

		    return Ok();
	    }
	}
}
