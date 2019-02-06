using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.UserGroup;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;
using CloudPlus.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CloudPlus.Api.Office365.Controllers.UserGroup
{
    public class Office365GroupController : ApiController
    {
       
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public Office365GroupController(IPowerShellUtility powerShellUtility,
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader)
        {
            
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        [Route("O365Group/CreateGroups", Name = "CreateGroups")]
        public IHttpActionResult CreateGroups([FromBody] DistributionGroupModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddO365Group);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("O365Group/CreateGroupMembers", Name = "CreateGroupMembers")]
        public IHttpActionResult CreateGroupMembers([FromBody] DistributionGroupMembersModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddO365GroupMember);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("O365Group/RemoveGroupMember", Name = "RemoveGroupMember")]
        public IHttpActionResult RemoveGroupMember([FromBody] DistributionGroupMembersModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveO365GroupMember);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("O365Group/RemoveGroups", Name = "RemoveGroups")]
        public IHttpActionResult RemoveGroups([FromBody] RemoveDistributionGroupModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveO365Group);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
    }
}