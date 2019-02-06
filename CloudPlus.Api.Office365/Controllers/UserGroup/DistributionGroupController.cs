using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.UserGroup;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;
using CloudPlus.Resources;

namespace CloudPlus.Api.Office365.Controllers.UserGroup
{
    public class DistributionGroupController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public DistributionGroupController(IPowerShellUtility powerShellUtility,
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader)
        {
            _powerShellUtility = powerShellUtility;
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
        }

        [HttpPost]
        [Route("DistributionGroup/CreateGroups", Name = "CreateGroups")]
        public IHttpActionResult CreateGroups([FromBody] DistributionGroupModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddDistributionGroup);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("DistributionGroup/AddGroupMembers", Name = "CreateGroupMembers")]
        public IHttpActionResult CreateGroupMembers([FromBody] DistributionGroupMembersModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddDistributionGroupMember);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("DistributionGroup/RemoveGroupMember", Name = "RemoveGroupMember")]
        public IHttpActionResult RemoveGroupMember([FromBody] DistributionGroupMembersModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveDistributionGroupMember);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("DistributionGroup/RemoveGroups", Name = "RemoveGroups")]
        public IHttpActionResult RemoveGroups([FromBody] RemoveDistributionGroupModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveDistributionGroup);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }
    }
}