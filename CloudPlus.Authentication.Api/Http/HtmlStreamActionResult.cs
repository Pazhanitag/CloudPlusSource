using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CloudPlus.Authentication.Api.Http
{
    public class HtmlStreamActionResult : IHttpActionResult
    {
        private readonly Func<Task<Stream>> _viewStream;

        public HtmlStreamActionResult(Func<Task<Stream>> viewStream)
        {
            _viewStream = viewStream;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var stream = await _viewStream();

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html")
            {
                CharSet = Encoding.UTF8.WebName
            };

            return response;
        }
    }
}