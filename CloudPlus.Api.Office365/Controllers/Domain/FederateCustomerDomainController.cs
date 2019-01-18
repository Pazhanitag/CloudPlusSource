using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.Domain;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.Domain
{
    public class FederateCustomerDomainController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;

        public FederateCustomerDomainController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]Office365CustomerDomainModelWithCredentials model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.FederateCustomerDomain);

            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok(true);
        }
    }
}
