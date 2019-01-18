using System.Linq;
using System.Web.Http;
using CloudPlus.Api.ViewModels.Response.Roles;
using CloudPlus.Services.Identity.Role;

namespace CloudPlus.Api.Controllers.Utilities
{
	[Authorize]
	[RoutePrefix("api/rpc/rolesutilities")]
	public class RolesUtilitiesController : ApiController
    {
	    private readonly IRoleService _roleService;

	    public RolesUtilitiesController(IRoleService roleService)
	    {
		    _roleService = roleService;
	    }

	    [HttpGet]
		[AllowAnonymous]
		[Route("GetAllRoles")]
	    public IHttpActionResult GetAllRoles()
	    {
		    var response = _roleService.GetAllRoles();

		    return Ok(response.Select(r => new RoleViewModel
		    {
			    Id = r.Id,
			    Name = r.Name
		    }));
	    }
	}
}
