using System.Threading.Tasks;
using CloudPlus.Enums.Provisions;
using CloudPlus.Models.Provisions;

namespace CloudPlus.Services.Database.Provisions
{
	public interface IProvisioningService
	{
	    Task<CompanyProvisioningStatus> GetComapnyProvisionedStatusAsync(int companyId, int productId);
		Task ProvisionAsync(int companyId, int productId);
		Task<bool> DeProvisionAsync(int companyId, int productId);
	    Task<bool> UpdateStatusAsync(int companyId, int productId, CompanyProvisioningStatus status);
        Task<ProvisionedServiceModel> GetServicesProvisionedToUser(string username, int companyId);
    }
}
