using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloudPlus.Api.ActiveDirectory.Extensions;
using CloudPlus.Api.ActiveDirectory.Models.User;
using CloudPlus.Api.ActiveDirectory.Utils;
using CloudPlus.Logging;
using CloudPlus.PowerShell;
using CloudPlus.Resources;

namespace CloudPlus.Api.ActiveDirectory.Controllers.User
{
    public class UserController : ApiController
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly ISamAccountNameGenerator _samAccountNameGenerator;

        public UserController(IConfigurationManager configurationManager,
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            ISamAccountNameGenerator samAccountNameGenerator)
        {
            _configurationManager = configurationManager;
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _samAccountNameGenerator = samAccountNameGenerator;
        }

	    [HttpGet]
	    public IHttpActionResult GetUser(string upn)
	    {
		    var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetUser);

		    _powerShellManager.AddParameter("upn", upn);
		    var response = _powerShellManager.ExecuteScriptAndReturnFirst<object>(script);

		    if (response == null)
		    {
			    return NotFound();
		    }

		    return Ok(response);
	    }

        [HttpPost]
        public IHttpActionResult Post([FromBody]CreateUser user)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.CreateUser);

            var samAccountName = _samAccountNameGenerator.GenerateSamAccountName(user.Upn);

            _powerShellManager.AttachParameters(user.MapPropertiesToActiveDirectoryParameters());

            _powerShellManager
                .AddParameter("samAccountName", samAccountName)
                .AddParameter("baseHostingOu", _configurationManager.GetByKey("BaseHostingOu"))
                .AddParameter("targetDomainController", _configurationManager.GetByKey("TargetDomainController"));

            _powerShellManager.ExecuteScript(script);

            return Ok(new UserCreated
            {
                SamAccountName = samAccountName
            });
        }

        [HttpDelete]
        public IHttpActionResult Delete(string upn)
        {
            if (string.IsNullOrWhiteSpace(upn))
                throw new ArgumentException("Upn cannot be null or empty");

            var script = _powershellScriptLoader.LoadScript(PowershellScripts.DeleteUser);
            _powerShellManager
                .AddParameter("upn", upn);
            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
        [HttpPut]
        public IHttpActionResult Put([FromBody]UpdateUser user)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.UpdateUser);

            _powerShellManager.AttachParameters(user.MapPropertiesToActiveDirectoryParameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
    }
}
