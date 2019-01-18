using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.Domain;
using CloudPlus.Api.Office365.Models.Transition;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.Logging;
using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Controllers.Transition
{
    [RoutePrefix("Transition")]
    public class TransitionsController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public TransitionsController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        [Route("GetMatchingData", Name = "GetMatchingData")]
        public IHttpActionResult GetMatchingData([FromBody] Office365CustomerDomainModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetTransitionMatchingData);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellUtility.AttachSqlHostAndCredentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            var list = new List<TransitionMatchingDataModel>();

            try
            {
                var result = _powerShellManager.ExecuteScriptAndReturnFirst<string>(script);

                if (result == null) return Ok(list);

                try
                {
                    return Ok(JsonConvert.DeserializeObject<List<TransitionMatchingDataModel>>(result));
                }
                catch (Exception )
                {
                    list.Add(JsonConvert.DeserializeObject<TransitionMatchingDataModel>(result));

                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error($"Error GetMatchingData: Office 365 CustomerId: {model.Office365CustomerId}, Domain: {model.Domain}", ex);

                return Ok(list);
            }
        }
    }
}
