using System.Web.Http;
using CloudPlus.Api.ActiveDirectory.Models.Company;
using CloudPlus.Logging;
using CloudPlus.PowerShell;
using CloudPlus.Resources;

namespace CloudPlus.Api.ActiveDirectory.Controllers.Company
{
    public class CompanyController : ApiController
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;

        public CompanyController(IConfigurationManager configurationManager, IPowerShellManager powerShellManager, IPowershellScriptLoader powershellScriptLoader)
        {
            _configurationManager = configurationManager;
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]CreateCompany company)
        {
            this.Log().Info("=> Post");

            var script = _powershellScriptLoader.LoadScript(PowershellScripts.CreateCompany);

            _powerShellManager
                .AddParameter("customerAccountId", company.CompanyOu)
                .AddParameter("baseHostingOu", _configurationManager.GetByKey("BaseHostingOu"))
                .AddParameter("targetDomainController", _configurationManager.GetByKey("TargetDomainController"));

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
    }
}
