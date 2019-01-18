using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.Domain;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.Customer
{
    [RoutePrefix("Customers")]
    public class CustomersController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public CustomersController(
            IPowerShellManager powerShellManager, 
            IPowershellScriptLoader powershellScriptLoader, 
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        [Route("GetCustomerIdByDomain", Name = "GetCustomerIdByDomain")]
        public IHttpActionResult GetCustomerIdByDomain([FromBody] Office365CustomerDomainModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetCustomerIdByDomain);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            var customerId = _powerShellManager.ExecuteScriptAndReturnFirst<string>(script);

            return Ok(customerId);
        }
    }
}
