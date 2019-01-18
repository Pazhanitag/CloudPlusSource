using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.User;
using CloudPlus.Api.ViewModels.Response.User;
using CloudPlus.Constants;
using CloudPlus.Enums.Notification;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.Services.ActiveDirectory;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Services.Identity.Password;
using CloudPlus.Services.Identity.User;
using CloudPlus.QueueModels.Users.Commands;

namespace CloudPlus.Api.Controllers.Utilities
{
    [Authorize]
    [RoutePrefix("api/rpc/userutilities")]
    public class UserUtilitiesController : ApiController
    {
        private readonly IWorkflowUserActivityService _workflowUserActivityService;
        private readonly IPasswordService _passwordService;
        private readonly IMessageBroker _messageBroker;
        private readonly IUserService _userService;
        private readonly IActiveDirectoryService _activeDirectoryService;

        public UserUtilitiesController(
            IWorkflowUserActivityService workflowUserActivityService,
            IPasswordService passwordService,
            IMessageBroker messageBroker,
            IUserService userService,
            IActiveDirectoryService activeDirectoryService)
        {
            _workflowUserActivityService = workflowUserActivityService;
            _passwordService = passwordService;
            _messageBroker = messageBroker;
            _userService = userService;
            _activeDirectoryService = activeDirectoryService;
        }

        [HttpGet]
        [Route("emailAvailable")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> EmailAvailable(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"{nameof(email)} is null or empty");

            var existingUser = await _userService.GetUserAsync(email);

            return Ok(existingUser == null && !_workflowUserActivityService.IsUserBeingCreated(email));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewUsers, PermissionsConstants.EditMyProfile)]
		[Route("displayNameAvailable")]
        public async Task<IHttpActionResult> DisplayNameAvailable(string displayName, int? userId = null,int? companyId=null)
        {
            var name = userId.HasValue;
            if (userId.HasValue && companyId.HasValue)
            {
                var user = await _userService.GetUserAsync(userId.Value,companyId.Value);
                if (user.DisplayName == displayName)
                {
                    return Ok(true);
                }
            }
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException($"{nameof(displayName)} is null or empty");

            var displayNameAvailable = await _userService.IsDisplayNameAvailable(displayName, User.CompanyId());

            return Ok(displayNameAvailable && !_workflowUserActivityService.IsUserBeingCreated(displayName, User.CompanyId()));
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("sendforgotpasswordemail", Name = "SendForgotPasswordEmail")]
        public async Task<IHttpActionResult> SendForgotPasswordEmail(ForgotPasswordViewModel model)
        {
            var sendEmailQueue = NotificationServiceConstants.QueueSendEmailNotification;
            var resetLink = await _passwordService.GetPasswordResetLink(model.Id, model.Email);

            await _messageBroker.GetSendEndpoint(sendEmailQueue).Send<ISendEmailCommand>(
                new
                {
                    UserName = model.Username,
                    TempResetLink = resetLink,
                    To = model.Email,
                    EmailTemplateType = EmailTemplateType.ForgotPassword,
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserEmails")]
        public async Task<IHttpActionResult> GetUserEmails(string email)
        {
            var user = await _userService.GetUserAsync(email) ?? await _userService.GetUserByAlternativeEmailAsync(email);

            if (user == null)
                return NotFound();

            return Ok(user.ToUserEmailsViewModel());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("isemailvalid")]
        public async Task<IHttpActionResult> IsEmailValid(string email)
        {
            var emailValid = await _userService.IsEmailValid(email);

            return Ok(emailValid);
        }

        [HttpPut]
        [Route("updatepassword", Name = "UpdatePassword")]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            var user = await _userService.GetUserAsync(model.UserId);

            if (user == null)
                return NotFound();


            var confirmationTokenResponse = await _passwordService.IsConfirmationTokenValid(user.Email, model.Token);

            if (!confirmationTokenResponse) return BadRequest("Confirmation token is invalid");

            await _activeDirectoryService.UpdateUserPassword(user.Email, model.Password);

            _userService.ResetSecurityStamp(user.UserName);

            return Ok(true);
        }

        [HttpPut]
        [Route("changepassword", Name = "ChangePassword")]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userService.GetUserAsync(model.UserId);

            if (user == null)
                return NotFound();

            if (user.CompanyId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var changeUserPasswordQueue = UserServiceConstants.QueueChangeUserPassword;

            await _messageBroker.GetSendEndpoint(changeUserPasswordQueue)
                .Send<IChangeUserPasswordCommand>(
                    model.ToChangeUserPasswordCommand()
                );

            return Ok();
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
		[Route("getparentprofiledata", Name = "GetParentProfileData")]
        public async Task<IHttpActionResult> GetParentProfileData()
        {
            if (!User.ParentId().HasValue)
                return StatusCode(HttpStatusCode.BadRequest);

            var user = await _userService.GetUserAsync(User.ParentId().Value);

            return Ok(user.ToUserViewModel(Url.Content("~/")));
        }
    }
}
