using System;
using System.Linq;
using System.Web.Http;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Api.Office365.Models.UserGroup;
using System.Collections.Generic;
using Newtonsoft.Json;
using CloudPlus.Logging;

namespace CloudPlus.Api.Office365.Controllers.UserGroup
{
    public class SecurityGroupController : ApiController
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;
        private readonly IPowerShellUtility _powerShellUtility;

        public SecurityGroupController(
            IPowerShellManager powerShellManager,
            IPowershellScriptLoader powershellScriptLoader,
            IPowerShellUtility powerShellUtility)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
            _powerShellUtility = powerShellUtility;
        }

        [HttpPost]
        [Route("securityGroup/GetAllGroups", Name = "GetAllGroups")]
        public IHttpActionResult GetAllGroups([FromBody] GroupsModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetGroupsListAllTypes);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            var list = new List<GroupsListModel>() { };
            try
            {
                var result = _powerShellManager.ExecuteScript<string>(script);
                if (result == null) return Ok(list);

                try
                {
                    return Ok(result);
                    //return Ok(JsonConvert.DeserializeObject<List<GroupsListModel>>(result));
                }
                catch (Exception)
                {
                   // list.Add(JsonConvert.DeserializeObject<GroupsListModel>(result));

                    return Ok(list);
                }
            }
            catch(Exception ex)
            {
                this.Log().Error($"Error GetGroups: Office 365 CustomerId: {model.Office365CustomerId}", ex);

                return Ok(list);
            }
            //var removedEmptyRoles = roles.Where(r => r.Length > 0);

          
        }


        [HttpPost]
        [Route("securityGroup/GetUserGroupMember", Name = "GetUserGroupMember")]
        public IHttpActionResult GetUserGroupMember([FromBody] GetUserGroupMembershipsModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.GetUserGroupMemberships);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("securityGroup/CreateGroups", Name = "CreateGroups")]
        public IHttpActionResult CreateGroups([FromBody] SecurityGroupModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddSecurityGroup);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("securityGroup/AddGroupMember", Name = "CreateGroupMember")]
        public IHttpActionResult CreateGroupMember([FromBody]SecurityGroupMemberModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.AddSecurityGroupMember);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("securityGroup/RemoveGroupMember", Name = "RemoveGroupMember")]
        public IHttpActionResult RemoveGroupMember([FromBody] SecurityGroupMemberModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveSecurityGroupMember);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        [HttpPost]
        [Route("securityGroup/RemoveGroups", Name = "RemoveGroups")]
        public IHttpActionResult RemoveGroups([FromBody] SecurityGroupModel model)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.RemoveSecurityGroup);

            _powerShellUtility.AttachOffice365Credentials(_powerShellManager);
            _powerShellManager.AttachParameters(model.MapPropertiesToOffice365Parameters());

            _powerShellManager.ExecuteScript(script);

            return Ok();
        }

        



    }
}