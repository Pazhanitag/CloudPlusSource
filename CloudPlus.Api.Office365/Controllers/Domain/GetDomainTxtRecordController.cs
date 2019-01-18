using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.Domain;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.Logging;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.Domain
{
    public class GetDomainTxtRecordController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public GetDomainTxtRecordController(
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
            this.Log().Info("=> GetDomainTxtRecordController");

            var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetDomainTxtRecord);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            this.Log().Info("Start executing script!");
            var txtRecord = _powerShellManager.ExecuteScriptAndReturnFirst<string>(script);

            this.Log().Info($"TXT Record: {txtRecord}");

            return Ok(txtRecord);
        }
    }
}
