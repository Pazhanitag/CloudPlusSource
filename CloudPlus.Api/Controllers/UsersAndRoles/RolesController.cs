using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Constants;
using CloudPlus.Services.Identity.Role;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Api.Controllers.UsersAndRoles
{
    [Authorize]
    [RoutePrefix("api/roles")]
    public class RolesController : ApiController
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RolesController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }
       
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewUsers)]
		public async Task<IHttpActionResult> GetAllRoles()
        {
            var user = await _userService.GetUserAsync(User.UserId());
            
            if(user == null)
                return InternalServerError(new Exception("Could not find user"));

            var userRole = user.Roles.FirstOrDefault();

            if (userRole == null)
                return InternalServerError(new Exception("User does not have any roles"));

            var response = await _roleService.GetAvailableRolesAsync(userRole.Id);

            return Ok(response.Select(r => r.ToRoleViewModel()));
        }
    }
}
