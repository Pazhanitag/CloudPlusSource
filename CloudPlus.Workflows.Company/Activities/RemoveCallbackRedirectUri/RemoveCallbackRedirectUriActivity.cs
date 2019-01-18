using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Identity.Client;
using CloudPlus.Workflows.Office365.Activities.Identity.RemoveCallbackRedirectUri;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.RemoveCallbackRedirectUri
{
    public class RemoveCallbackRedirectUriActivity : IRemoveCallbackRedirectUriActivity
    {
        private readonly IClientService _clientService;

        public RemoveCallbackRedirectUriActivity(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveCallbackRedirectUriArguments> context)
        {
            var arguments = context.Arguments;

            await _clientService.RemoveRedirectUri(arguments.Uri, arguments.ClientDbId);

            return context.Completed(new RemoveCallbackRedirectUriLog
            {
                Uri = arguments.Uri,
                ClientDbId = arguments.ClientDbId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IRemoveCallbackRedirectUriLog> context)
        {
            try
            {
                var logs = context.Log;

                await _clientService.AddRedirectUri(logs.Uri, logs.ClientDbId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating RemoveCallbackRedirectUriActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
