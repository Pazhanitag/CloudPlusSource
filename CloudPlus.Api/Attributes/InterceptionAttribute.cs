using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MassTransit;
using Microsoft.Owin;

namespace CloudPlus.Api.Attributes
{
    public class InterceptionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            GetOwinContext().Set("RequestGUID", NewId.NextGuid());
            base.OnActionExecuting(actionContext);
        }

        public virtual IOwinContext GetOwinContext()
        {
            return HttpContext.Current.GetOwinContext();
        }
    }
}