using System.Web.Http;
using CloudPlus.Api.ActiveDirectory.Extensions;
using CloudPlus.Api.ActiveDirectory.Models.User;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.ActiveDirectory.Controllers.User
{
    public class UserPasswordController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;

        public UserPasswordController(IPowerShellManager powerShellManager, IPowershellScriptLoader powershellScriptLoader)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
        }
        [HttpPut]
        public IHttpActionResult Put([FromBody] ChangeUserPassword changeUserPassword)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.ChangeUserPassword);

            _powerShellManager.AttachParameters(changeUserPassword.MapPropertiesToActiveDirectoryParameters());
            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
    }
}