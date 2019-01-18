using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CloudPlus.Infrastructure.Http.Handlers
{
    public class ResponseMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.ToString().ToLower().Contains("/swagger/") || request.RequestUri.ToString().ToLower().Contains("forgotpassword"))
                return await base.SendAsync(request, cancellationToken);

            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            response.TryGetContentValue(out object content);

            if (!response.IsSuccessStatusCode)
            {
                if (content is HttpError errors)
                {

                    if (errors.ModelState?.Values.Any() == true)
                        return request.CreateResponse(response.StatusCode,
                            new HttpResponse(null, errors.ModelState.Values.SelectMany(s => s as string[]).ToList()));

                    var errorMsg = "";
                    foreach (var error in errors)
                    {
                        errorMsg += $"{error.Key}: {error.Value} {Environment.NewLine}";
                    }
                    return request.CreateResponse(response.StatusCode, new HttpResponse(null, new List<string>
                    {
                        $"Errors: {Environment.NewLine} {errorMsg}"
                    }));
                }
            }
            //Handing the response for download excel and other.If the response file has excel file then we send the response as it is (which is already a httpresponcemessage)
            //    Tag Dev

            if (response?.Content?.Headers?.ContentType?.MediaType != null)
            {
                if (response.Content.Headers.ContentType.MediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                { return response; }
            }

            //Tag Dev End
            var res = request.CreateResponse(response.StatusCode, new HttpResponse(content));

            foreach (var header in response.Headers)
                res.Headers.Add(header.Key, header.Value);

            return res;
        }
    }
}
