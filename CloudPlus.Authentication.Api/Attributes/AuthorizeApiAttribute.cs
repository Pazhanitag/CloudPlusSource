using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CloudPlus.Authentication.Api.Attributes
{
    public class AuthorizeApiAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!(filterContext.RequestContext.Principal is ClaimsPrincipal principal))
                throw new ArgumentNullException(nameof(ClaimsPrincipal));

            if (principal.Identity.IsAuthenticated && principal.HasClaim("scope", "trustedAPI"))
            {
                base.OnAuthorization(filterContext);

                return;
            }

            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            base.OnAuthorization(filterContext);
        }
    }
}