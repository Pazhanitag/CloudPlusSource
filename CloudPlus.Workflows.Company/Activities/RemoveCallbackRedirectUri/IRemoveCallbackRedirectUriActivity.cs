using CloudPlus.Workflows.Office365.Activities.Identity.RemoveCallbackRedirectUri;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.RemoveCallbackRedirectUri
{
    public interface IRemoveCallbackRedirectUriActivity : Activity<IRemoveCallbackRedirectUriArguments, IRemoveCallbackRedirectUriLog>
    {
    }
}
