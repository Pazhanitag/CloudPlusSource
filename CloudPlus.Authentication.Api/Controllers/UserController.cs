using System;
using System.Web.Http;

namespace CloudPlus.Authentication.Api.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        public UserController()
        {
        }
        //TODO This methods should be removed and global messeging used instead
        internal IHttpActionResult SetErrorResult(Exception exception)
        {
            ModelState.AddModelError(string.Empty, exception.InnerException?.Message ?? exception.Message);
            return BadRequest(ModelState);
        }
    }
}
