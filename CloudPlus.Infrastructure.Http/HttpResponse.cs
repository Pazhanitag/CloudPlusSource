using System.Collections.Generic;

namespace CloudPlus.Infrastructure.Http
{
    public class HttpResponse
    {
        public List<string> ErrorMessage { get; set; }

        public object Result { get; set; }

        public HttpResponse(object result = null, List<string> errorMessage = null)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }
    }
}
