using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.User;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Api.Controllers.UsersAndRoles
{
    [Authorize]
    [RoutePrefix("api/profile")]
    public class ProfileController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMessageBroker _messageBroker;
        private readonly IImageHelper _imageHelper;

        public ProfileController(
            IUserService userService,
            IMessageBroker messageBroker,
            IImageHelper imageHelper)
        {
            _userService = userService;
            _messageBroker = messageBroker;
            _imageHelper = imageHelper;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewUsers, PermissionsConstants.EditMyProfile)]
		[Route(Name = "GetUserProfile")]
        public async Task<IHttpActionResult> GetUserProfile()
        {
            var user = await _userService.GetUserAsync(User.UserId());

            if (user == null)
                return NotFound();

            return Ok(user.ToUserProfileViewModel(Url.Content("~/")));
        }

        [HttpPut]
        [AuthorizeAccess(PermissionsConstants.EditUsers, PermissionsConstants.EditMyProfile)]
		[Route(Name = "UpdateUserProfile")]
        public async Task<IHttpActionResult> UpdateUserProfile(UpdateUserProfileViewModel model)
        {
            model.ProfilePicture = _imageHelper.SaveProfilePicture(model.AvatarBase64);

            var createUserQueue = UserServiceConstants.QueueUpdateUser;

            await _messageBroker.GetSendEndpoint(createUserQueue)
                .Send<IUpdateUserCommand>(
                    model.ToUpdateUserCommand()
                );
            return Ok();
        }

    }
}
