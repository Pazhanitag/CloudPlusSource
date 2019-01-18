using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.User;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.User
{
    public class HardDeleteUserController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public HardDeleteUserController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]HardDeleteUserModel user)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.HardDeleteUser);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(user.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok(true);
        }
    }
}
