using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloudPlus.Api.Security;
using Microsoft.AspNet.Identity;

namespace CloudPlus.Api.Attributes
{
    public class AuthorizeAccess : AuthorizeAttribute
    {
        private IAuthorizationManager _authorizationManager;
        private string[] Permissions { get; set; }

	    public AuthorizeAccess(params string[] permissionStrings)
	    {
		    Permissions = permissionStrings;
	    }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            _authorizationManager = actionContext.Request.GetDependencyScope()
                .GetService(typeof(IAuthorizationManager)) as IAuthorizationManager;

            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
                Forbidden(actionContext, cancellationToken);

            var userId = actionContext.RequestContext.Principal.Identity.GetUserId<int>();

            if (userId == 0)
            {
                Forbidden(actionContext, cancellationToken);

                return;
            }

            if (!AccessByPermission(userId))
            {
                Forbidden(actionContext, cancellationToken);

                return;
            }

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        private void Forbidden(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);

            base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        private bool AccessByPermission(int userId)
        {
            if (string.IsNullOrWhiteSpace(Roles) && Permissions.Any())
                return  _authorizationManager.HasAccessByPermission(Permissions, userId);

            return true;
        }
    }
}