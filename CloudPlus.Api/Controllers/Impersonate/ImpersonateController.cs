using CloudPlus.Api.Attributes;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Company;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Extensions;
using CloudPlus.Constants;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Api.Controllers.Impersonate
{
    [RoutePrefix("api/impersonate")]
    public class ImpersonateController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;

        public ImpersonateController(
            ICompanyService companyService, 
            IUserService userService)
        {
            this.Log().Info("ImpersonateController constructor");

            _companyService = companyService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
		[Route("{id:int}")]
        public async Task<IHttpActionResult> ImpersonateUser(int id)
        {
            this.Log().Info("===> ImpersonateUser");

            var authenticatedUserId = User.UserId();

            var authenticatedUser = await _userService.GetUserAsync(authenticatedUserId);

            if (authenticatedUser == null)
            {
                return BadRequest();
            }

            var impersonateUser = await _userService.GetUserAsync(id);

            if (impersonateUser == null)
            {
                return BadRequest();
            }

            if (!IsMemberInCompanyHierarchy(authenticatedUser.CompanyId, impersonateUser.CompanyId))
            {
                return BadRequest();
            }

            this.Log().Info("<=== ImpersonateUser");

            await _userService.GenerateImpersonateToken(authenticatedUserId, id);

            return Ok();
        }

        private bool IsMemberInCompanyHierarchy(int parentUserCompanyId, int impersonateUserCompanyId)
        {
            this.Log().Info("===> IsMemberInCompanyHierarchy");

            var isMember = _companyService.IsMemberInCompanyHierarchy(parentUserCompanyId, impersonateUserCompanyId);

            this.Log().Info($"isMember: {isMember}");
            this.Log().Info("<=== IsMemberInCompanyHierarchy");

            return isMember;
        }
    }
}
