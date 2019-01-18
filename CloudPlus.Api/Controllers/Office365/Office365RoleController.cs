using System.Linq;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Mappers;
using CloudPlus.Constants;
using CloudPlus.Services.Database.Office365.Role;

namespace CloudPlus.Api.Controllers.Office365
{
    [Authorize]
    [RoutePrefix("api/Office365Roles")]
    public class Office365RoleController : ApiController
    {
        private readonly IOffice365DbRoleService _office365RolesService;

        public Office365RoleController(IOffice365DbRoleService office365RolesService)
        {
            _office365RolesService = office365RolesService;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		[Route("GetAllRoles", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _office365RolesService.GetAllRoles();

            return Ok(roles.Select(r => r.ToOffice365RoleViewModel()));
        }
    }
}
