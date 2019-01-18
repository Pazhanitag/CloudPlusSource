using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Http.Filters;
using CloudPlus.Logging;
using Microsoft.Owin;

namespace CloudPlus.Api.Attributes
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //UNCOMMENT AND ADJUST THE CODE BELOW IF THERE IS A NEED FOR SPECIAL TREATMENT FOR CERTAIN TYPE OF EXCEPTION
            //
            //if (context.Exception is BusinessException)
            //{
            //  throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //  {
            //      Content = new StringContent(context.Exception.Message),
            //      ReasonPhrase = "Exception"
            //  });

            //}

            //LOG THE ERROR
            Debug.WriteLine(context.Exception);

            var exceptionType = context.Exception.GetType();
            var cpLogManager = LoggerExtension.GetLogger(exceptionType);

            //Send a JSON object
            var items = new Dictionary<string, string>
            {
                {"RequestGUID", GetOwinContext().Get<Guid>("RequestGUID").ToString()},
                {"ActionContext", context.ActionContext?.ToString() ?? ""},
                {"Exception", context.Exception?.ToString()},
                {"Request", context.Request?.ToString() ?? ""},
                {"Response", context.Response?.ToString() ?? ""}
            };
            cpLogManager.Error(items);

            //UNCOMMENT AND ADJUST THE CODE BELOW IF THERE IS A NEED FOR RE-THROWING THE EXCEPTION
            //
            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //{
            //  Content = new StringContent("An error occurred, please try again or contact the administrator."),
            //  ReasonPhrase = "Critical Exception"
            //});
        }

        public virtual IOwinContext GetOwinContext()
        {
            return HttpContext.Current.GetOwinContext();
        }
    }
}