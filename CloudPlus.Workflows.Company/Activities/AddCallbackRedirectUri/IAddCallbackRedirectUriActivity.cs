using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri
{
    public interface IAddCallbackRedirectUriActivity : Activity<IAddCallbackRedirectUriArguments, IAddCallbackRedirectUriLog>
    {
    }
}
