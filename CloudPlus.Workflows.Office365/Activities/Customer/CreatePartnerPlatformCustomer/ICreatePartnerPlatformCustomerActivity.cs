using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreatePartnerPlatformCustomer
{
    public interface ICreatePartnerPlatformCustomerActivity : Activity<ICreatePartnerPlatformCustomerArguments, ICreatePartnerPlatformCustomerLog>
    {
    }
}