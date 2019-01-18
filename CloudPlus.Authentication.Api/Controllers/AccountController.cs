using CloudPlus.Authentication.Api.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Authentication.Api.Attributes;
using CloudPlus.Services.Identity;
using CloudPlus.Services.Identity.User;
using ITokenProviderService = CloudPlus.Authentication.Identity.Services.ITokenProviderService;

namespace CloudPlus.Authentication.Api.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        private readonly ITokenProviderService _identityService;
        private readonly IUserService _userService;
        public AccountController(ITokenProviderService identityService, IUserService userService)
        {
            _identityService = identityService;
            _userService = userService;
        }

        /// <summary>
        /// Get password reset token
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("getpasswordresettoken")]
        [ValidateModel]
        public async Task<IHttpActionResult> GetPasswordResetToken(string userEmail)
        {
            return Ok(await _identityService.GenerateConfirmationToken(userEmail));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("validatePasswordResetToken")]
        [ValidateModel]
        public async Task<IHttpActionResult> ValidatePasswordResetToken([FromBody] ResetPasswordValidationModel model)
        {
            return Ok(await _identityService.IsConfirmationTokenValid(model.UserEmail, model.Token));
        }
    }
}