using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Models.Permissions;
using CloudPlus.Models.RolePermissions;
using CloudPlus.Models.Roles;
using CloudPlus.Models.UserRoles;
using CloudPlus.Models.Users;
using CloudPlus.Resources;
using ClousPlus.Api.Attributes;
using log4net;
using UserResponse = CloudPlus.Models.ResponseDto<CloudPlus.Models.Users.UserResponseDto>;
using RoleResponse = CloudPlus.Models.ResponseDto<CloudPlus.Models.Roles.RoleResponseDto>;
using PermissionResponse = CloudPlus.Models.ResponseDto<CloudPlus.Models.Permissions.PermissionResponseDto>;
using PagedRoleResult = CloudPlus.Models.ResponseDto<CloudPlus.Models.Paging.PagedResultContent<CloudPlus.Models.Roles.RoleResponseDto>>;
using PagedUserResult = CloudPlus.Models.ResponseDto<CloudPlus.Models.Paging.PagedResultContent<CloudPlus.Models.Users.UserResponseDto>>;
using System.Web;

namespace ClousPlus.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationManager _configurationManager;

        public UsersController(IHttpClientResolver httpClientResolver, IConfigurationManager configurationManager)
        {
            _httpClient = httpClientResolver.GetIdentityServerHttpClient();

            _configurationManager = configurationManager;
        }

        [HttpGet]
        [Route("{page:int}/{pageSize:int}", Name = "GetAllUsers")]
        public async Task<IHttpActionResult> GetAllUsers(int page, int pageSize)
        {
			var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.GetAllUsersEndpoint"), page,
                pageSize);
            
            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var users = response.Content.ReadAsAsync<PagedUserResult>().Result;

            users.Result.NextPageUrl = Url.Link("GetAllUsers", new
            {
                page = users.Result.PageNumber + 1,
                pageSize = pageSize
            });

            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUserById(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.GetUserByIdEndpoint"), id);

            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var user = response.Content.ReadAsAsync<UserResponse>().Result;

            user.Result.Url = Url.Link("GetUserById", new { id = user.Result.Id });

            return Ok(user);
        }

        [HttpGet]
        [Route("{id:int}/roles", Name = "GetUserByIdIncludeRoles")]
        public async Task<IHttpActionResult> GetUserByIdIncludeRoles(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.GetUserByIdIncludeRolesEndpoint"),
                id);

            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var user = response.Content.ReadAsAsync<UserResponse>().Result;

            user.Result.Url = Url.Link("GetUserByIdIncludeRoles", new { id = user.Result.Id });

            return Ok(user);
        }

        [HttpGet]
        [Route("{id:int}/permissions", Name = "GetUserByIdIncludeRolesAndPermissions")]
        public async Task<IHttpActionResult> GetUserByIdIncludeRolesAndPermissions(int id)
        {
            var controllerEndpoint = string.Format(
                _configurationManager.GetByKey("IS.GetUserByIdIncludeRolesAndPermissions"),
                id);

            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var user = response.Content.ReadAsAsync<UserResponse>().Result;

            user.Result.Url = Url.Link("GetUserByIdIncludeRolesAndPermissions", new { id = user.Result.Id });

            return Ok(user);
        }

        [HttpGet]
        [Route("", Name = "GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(string email)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.GetUserByEmail"), email);

            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var user = response.Content.ReadAsAsync<UserResponse>().Result;

            user.Result.Url = Url.Link("GetUserByEmail", new { id = user.Result.Id });

            return Ok(user);
        }

        [HttpPost]
        [ValidateModel]
        [Route("register")]
        public async Task<IHttpActionResult> CreateUser(UserRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.CreateUser");

            var response = await PostAsync(controllerEndpoint, model);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var user = response.Content.ReadAsAsync<UserResponse>().Result;

            user.Result.Url = Url.Link("GetUserById", new { id = user.Result.Id });

            return Ok(user);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IHttpActionResult> UpdateUser(UserRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.UpdateUser");

            var response = await PutAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.DeleteUser"), id);

            var response = await DeleteAsync(controllerEndpoint);

            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("roles/{page:int}/{pageSize:int}", Name = "GetAllRoles")]
        public async Task<IHttpActionResult> GetAllRoles(int page, int pageSize)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.GetAllRoles"), page, pageSize);

            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var roles = response.Content.ReadAsAsync<PagedRoleResult>().Result;

            roles.Result.NextPageUrl = Url.Link("GetAllRoles", new
            {
                page = roles.Result.PageNumber + 1,
                pageSize = pageSize
            });
 
            return Ok(roles);
        }

        [HttpPost]
        [ValidateModel]
        [Route("role")]
        public async Task<IHttpActionResult> CreateRole(RoleRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.CreateRole");

            var response = await PostAsync(controllerEndpoint, model);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var role = response.Content.ReadAsAsync<RoleResponse>().Result;

            return Ok(role);
        }

        [HttpPut]
        [ValidateModel]
        [Route("role")]
        public async Task<IHttpActionResult> UpdateRole(RoleRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.UpdateRole");

            var response = await PutAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpDelete]
        [Route("role/{id:int}")]
        public async Task<IHttpActionResult> DeleteRole(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.DeleteRole"), id);

            var response = await DeleteAsync(controllerEndpoint);

            return ResponseMessage(response);
        }

        [HttpPost]
        [ValidateModel]
        [Route("role/assign")]
        public async Task<IHttpActionResult> AssignUserToRole(UserRoleRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.AssignUserToRole");

            var response = await PostAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpPut]
        [ValidateModel]
        [Route("role/removefrom")]
        public async Task<IHttpActionResult> RemoveUserFromRole(UserRoleRequestDto model)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.RemoveUserFromRole"));

            var response = await PutAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpDelete]
        [Route("{id:int}/removefromroles")]
        public async Task<IHttpActionResult> RemoveAllUserRoles(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.RemoveUserFromRole"), id);

            var response = await DeleteAsync(controllerEndpoint);

            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("permission/{id:int}", Name = "GetPermissionById")]
        public async Task<IHttpActionResult> GetPermissionById(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.GetPermissionById"), id);

            var response = await GetAsync(controllerEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return ResponseMessage(response);
            }

            var permission = response.Content.ReadAsAsync<PermissionResponse>().Result;

            permission.Result.Url = Url.Link("GetUserByEmail", new { id = permission.Result.Id });

            return Ok(permission);
        }

        [HttpPost]
        [ValidateModel]
        [Route("permission")]
        public async Task<IHttpActionResult> CreatePermission(PermissionRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.CreatePermission");

            var response = await PostAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpPut]
        [ValidateModel]
        [Route("permission")]
        public async Task<IHttpActionResult> UpdatePermission(PermissionRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.UpdatePermission");

            var response = await PutAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpDelete]
        [ValidateModel]
        [Route("permission/{id:int}")]
        public async Task<IHttpActionResult> DeletePermission(int id)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.DeletePermission"), id);

            var response = await DeleteAsync(controllerEndpoint);

            return ResponseMessage(response);
        }

        [HttpPost]
        [ValidateModel]
        [Route("role/permission/assign")]
        public async Task<IHttpActionResult> AssignPermissionToRole(RolePermissionRequestDto model)
        {
            var controllerEndpoint = _configurationManager.GetByKey("IS.AssignPermissionToRole");

            var response = await PostAsync(controllerEndpoint, model);

            return ResponseMessage(response);
        }

        [HttpDelete]
        [ValidateModel]
        [Route("role/{roleId:int}/permission/{permissionId:int}")]
        public async Task<IHttpActionResult> DeleteRolePermission(int roleId, int permissionId)
        {
            var controllerEndpoint = string.Format(_configurationManager.GetByKey("IS.DeleteRolePermission"), roleId,
                permissionId);

            var response = await DeleteAsync(controllerEndpoint);

            return ResponseMessage(response);
        }

        private async Task<HttpResponseMessage> GetAsync(string controllerActionUrl)
        {
            var authenticationServiceEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var url = string.Concat(authenticationServiceEndpoint, controllerActionUrl);

            var response = await _httpClient.GetAsync(url);

            return response;
        }

        private async Task<HttpResponseMessage> PostAsync<T>(string controllerActionUrl, T model)
        {
            var authenticationServiceEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var url = string.Concat(authenticationServiceEndpoint, controllerActionUrl);
            
            var response = await _httpClient.PostAsJsonAsync(url, model);

            return response;
        }

        private async Task<HttpResponseMessage> PutAsync<T>(string controllerActionUrl, T model)
        {
            var authenticationServiceEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var url = string.Concat(authenticationServiceEndpoint, controllerActionUrl);

            var response = await _httpClient.PutAsJsonAsync(url, model);

            return response;
        }

        private async Task<HttpResponseMessage> DeleteAsync(string controllerActionUrl)
        {
            var authenticationServiceEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var url = string.Concat(authenticationServiceEndpoint, controllerActionUrl);

            var response = await _httpClient.DeleteAsync(url);

            return response;
        }
    }
}
