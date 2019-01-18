using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CloudPlus.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudPlus.Infrastructure.Http.Handlers
{
    public class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.ToString().ToLower().Contains("/swagger/"))
                return await base.SendAsync(request, cancellationToken);

            var corrId = request.GetCorrelationId().ToString();
            var requestInfo = $"{request.Method} {request.RequestUri}";

            var requestMessage = await request.Content.ReadAsByteArrayAsync();

            await Log(corrId, requestInfo, requestMessage, "Request");

            var response = await base.SendAsync(request, cancellationToken);

            var responseMessage = new byte[] { };

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content != null)
                    {
                        responseMessage = await response.Content.ReadAsByteArrayAsync();
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorMessage = $"{response.ReasonPhrase}:{Environment.NewLine}{FormatErrorMessage(error)}";
                    responseMessage = Encoding.UTF8.GetBytes(errorMessage);
                }
            }
            catch (System.Exception ex)
            {
                this.Log().Error("Logging error", ex);
            }

            await Log(corrId, requestInfo, responseMessage, "Response");

            return response;
        }
        private async Task Log(string correlationId, string requestInfo, byte[] message, string type)
        {
            var payload = "";

            try
            {
                var jsonPayload = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(message));
                jsonPayload?.Properties().Where(p => p.Name.ToLower().Contains("password")).ToList().ForEach(p => p.Value = "****");
                payload = JsonConvert.SerializeObject(jsonPayload);
            }
            catch (Exception)
            {
                payload = Encoding.UTF8.GetString(message);
            }
            var logObj = new
            {
                Type = type,
                CorrelationId = correlationId,
                RequestInfo = requestInfo,
                Payload = payload
            };
            
            await Task.Run(() => this.Log().Info(logObj));
        }

        private static string FormatErrorMessage(string json)
        {
            var errorMessage = "";
            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                errorMessage = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch (Exception)
            {
                errorMessage = json;
            }
            return errorMessage;
        }
    }
}
