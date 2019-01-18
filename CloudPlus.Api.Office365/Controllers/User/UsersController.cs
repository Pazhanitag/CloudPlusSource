using System;
using System.Linq;
using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.User;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.User
{
    public class UsersController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public UsersController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        [Route("users", Name = "CreateUser")]
        public IHttpActionResult CreateUser([FromBody] CreateUserModel user)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.O365CreateUser);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(user.MapPropertiesToOffice365Parameters());

            var office365UserId = _powerShellManager.ExecuteScriptAndReturnFirst<Guid>(script);

            return Ok(office365UserId);
        }

        [HttpPost]
        [Route("users/GetRoles", Name = "GetUserRoles")]
        public IHttpActionResult GetUserRoles([FromBody] UserRolesModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetUserRoles);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            var roles = _powerShellManager.ExecuteScript<string>(script);
            
            var removedEmptyRoles = roles.Where(r => r.Length > 0);

            return Ok(removedEmptyRoles);
        }

        [HttpPost]
        [Route("users/roles", Name = "AssignUserRoles")]
        public IHttpActionResult AssignUserRoles([FromBody] UserRolesModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AssignUserRoles);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("users/RemoveRoles", Name = "RemoveRoles")]
        public IHttpActionResult RemoveRoles([FromBody] UserRolesModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveUserRoles);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
    }
}
