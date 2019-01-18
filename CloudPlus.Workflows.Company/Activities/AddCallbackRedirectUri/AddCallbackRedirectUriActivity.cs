using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Identity.Client;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri
{
    public class AddCallbackRedirectUriActivity : IAddCallbackRedirectUriActivity
    {
        private readonly IClientService _clientService;

        public AddCallbackRedirectUriActivity(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAddCallbackRedirectUriArguments> context)
        {
            var arguments = context.Arguments;
            var redirectUri = $"http://{arguments.Uri}/static/callback.html";
            var silentRedirectUri = $"http://{arguments.Uri}/static/silent.html";
            var postLogoutRedirectUri = $"http://{arguments.Uri}";
            
            await _clientService.AddRedirectUri(redirectUri, arguments.ClientDbId);
            await _clientService.AddRedirectUri(silentRedirectUri, arguments.ClientDbId);
            await _clientService.AddPostLogoutRedirectUri(postLogoutRedirectUri, arguments.ClientDbId);
            
            return context.Completed(new AddCallbackRedirectUriLog
            {
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = postLogoutRedirectUri,
                SilentRedirectUri = silentRedirectUri,
                ClientDbId = arguments.ClientDbId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IAddCallbackRedirectUriLog> context)
        {
            try
            {
                var logs = context.Log;

                await _clientService.RemoveRedirectUri(logs.RedirectUri, logs.ClientDbId);
                await _clientService.RemoveRedirectUri(logs.SilentRedirectUri, logs.ClientDbId);
                await _clientService.RemovePostLogoutRedirectUri(logs.PostLogoutRedirectUri, logs.ClientDbId);

            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating AddCallbackRedirectUriActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
