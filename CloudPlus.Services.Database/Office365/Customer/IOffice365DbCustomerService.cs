using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Models.Office365.Customer;

namespace CloudPlus.Services.Database.Office365.Customer
{
    public interface IOffice365DbCustomerService
    {
        Task<Office365CustomerModel> CreateDatabaseCustomerAsync(Office365CustomerModel customer);
        Task DeleteDatabaseCustomerAsync(int id);
        Task<Office365CustomerModel> GetOffice365CustomerAsync(int companyId);
        Task<Office365CustomerModel> GetOffice365CustomerWithIncludesAsync(int companyId);
        Task<Office365CompanyModel> GetCompanyOffice365DomainsAsync(int companyId);
    }
}
