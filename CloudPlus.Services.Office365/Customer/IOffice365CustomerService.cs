using System.Threading.Tasks;
using CloudPlus.Models.Office365.Customer;

// ReSharper disable once CheckNamespace
namespace CloudPlus.Services.Office365.CustomerService
{
    public interface IOffice365CustomerService
    {
        Task<Office365PartnerPlatformCustomerModel> CreatePartnerPlatformCustomerAsync(Office365PartnerPlatformCustomerModel customer);
        Task<string> GetCustomerDefaultDomainAsync(string office365CustomerId);
    }
}
