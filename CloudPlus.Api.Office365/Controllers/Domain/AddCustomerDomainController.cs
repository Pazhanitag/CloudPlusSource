using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.Domain;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.Logging;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.Domain
{
    public class AddCustomerDomainController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public AddCustomerDomainController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]Office365CustomerDomainModel model)
        {
            this.Log().Info("=> AddCustomerDomainController");

            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddCustomerDomain);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            this.Log().Info("Start executing script!");
            _powerShellManager.ExecuteScript(script);

            return Ok(true);
        }
    }
}
