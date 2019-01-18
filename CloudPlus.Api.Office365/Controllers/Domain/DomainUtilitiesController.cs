using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.Domain;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.Domain
{
    [RoutePrefix("DomainUtilities")]
    public class DomainUtilitiesController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public DomainUtilitiesController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }


        [HttpPost]
        [Route("isdomainverified", Name = "IsDomainVerified")]
        public IHttpActionResult IsDomainVerified([FromBody]Office365CustomerDomainModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.IsDomainVerified);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            var verified = _powerShellManager.ExecuteScriptAndReturnFirst<bool>(script);

            return Ok(verified);
        }

        [HttpPost]
        [Route("isdomainfederated", Name = "IsDomainFederated")]
        public IHttpActionResult IsDomainFederated([FromBody]Office365CustomerDomainModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.IsDomainFederated);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            var verified = _powerShellManager.ExecuteScriptAndReturnFirst<bool>(script);

            return Ok(verified);
        }
    }
}
