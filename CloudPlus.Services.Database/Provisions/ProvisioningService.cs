using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities;
using CloudPlus.Enums.Office365;
using CloudPlus.Enums.Provisions;
using CloudPlus.Models.Provisions;
using CloudPlus.Services.Database.Office365.Domain;
using CloudPlus.Services.Database.Office365.License;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Database.Product;
using CloudPlus.Services.Database.WorkflowActivity.Office365;

namespace CloudPlus.Services.Database.Provisions
{
    public class ProvisioningService : IProvisioningService
    {
        private readonly CldpDbContext _dbContext;
        private readonly IOffice365DbDomainService _office365DomainService;
        private readonly IWorkflowOffice365ActivityService _workflowOffice365ActivityService;
        private readonly IOffice365DbUserService _office365UserService;
        private readonly IOffice365DbLicenseService _office365DbLicenseService;
        private readonly IProductService _productService;

        public ProvisioningService(
            CldpDbContext dbContext,
            IOffice365DbDomainService office365DomainService,
            IWorkflowOffice365ActivityService workflowOffice365ActivityService,
            IOffice365DbUserService office365UserService,
            IOffice365DbLicenseService office365DbLicenseService, 
            IProductService productService)
        {
            _dbContext = dbContext;
            _office365DomainService = office365DomainService;
            _workflowOffice365ActivityService = workflowOffice365ActivityService;
            _office365UserService = office365UserService;
            _office365DbLicenseService = office365DbLicenseService;
            _productService = productService;
        }

        public async Task<CompanyProvisioningStatus> GetComapnyProvisionedStatusAsync(int companyId, int productId)
        {
            var provision = await _dbContext.Provisions.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ProductV2Id == productId);

            return provision?.Status ?? CompanyProvisioningStatus.NotProvisioned;
        }

        public async Task ProvisionAsync(int companyId, int productId)
        {
            var alreadyProvisioned = await GetComapnyProvisionedStatusAsync(companyId, productId);

            if (alreadyProvisioned != CompanyProvisioningStatus.NotProvisioned)
                throw new Exception("Product already provisioned to the company!");

            try
            {
                _dbContext.Provisions.Add(new Provision
                {
                    ProductV2Id = productId,
                    CompanyId = companyId,
                    Status = CompanyProvisioningStatus.Provisioned,
                });

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error while trying to provision the product or service!", e);
            }
        }

        public async Task<bool> DeProvisionAsync(int companyId, int productId)
        {
            var provision = await _dbContext.Provisions
                .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ProductV2Id == productId);

            if (provision == null)
            {
                throw new Exception("Product already deprovisioned to the company!");
            }

            try
            {
                _dbContext.Provisions.Remove(provision);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error while trying to provision the product or service!", e);
            }

            return true;
        }

        public async Task<bool> UpdateStatusAsync(int companyId, int productId, CompanyProvisioningStatus status)
        {
            var provision = await _dbContext.Provisions
                .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ProductV2Id == productId);

            if (provision == null)
                throw new Exception("Product already deprovisioned to the company!");

            try
            {
                provision.Status = status;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error while trying to update company provision status!", e);
            }

            return true;
        }

        public async Task<ProvisionedServiceModel> GetServicesProvisionedToUser(string username, int companyId)
        {
            var companyProvisions = await _dbContext.Provisions.FirstOrDefaultAsync(p => p.CompanyId == companyId);

            if (companyProvisions == null) return null;

            var userService = new ProvisionedServiceModel
            {
                ProductId = companyProvisions.ProductV2Id,
                Status = UserProvisioningStatus.NotAvailable
            };

            var product = await _productService.GetProduct(userService.ProductId);
            userService.Name = product.Name;
            userService.Vendor = product.Vendor;
            userService.ImgUrl = product.ImgUrl;

            userService.Status = await GetUserOffice365ProvisioningStatus(username);

            if (userService.Status != UserProvisioningStatus.Assigned) return userService;

            var assignedLicense = await _office365DbLicenseService.GetUserAssgnedLicenseAsync(username);
            if (assignedLicense != null)
            {
                userService.AssignedLicense = product.ProductItems.FirstOrDefault(i =>
                    i.Identifier == assignedLicense.Office365Offer.CloudPlusProductIdentifier)?.Name;
            }

            return userService;
        }

        private async Task<UserProvisioningStatus> GetUserOffice365ProvisioningStatus(string username)
        {
            var isServiceAssignmentInProgress = _workflowOffice365ActivityService.IsOffice365UserLicenceAssignmentInProgress(username);
            if (isServiceAssignmentInProgress) return UserProvisioningStatus.InProgress;

            var isServiceChangingInProgress = _workflowOffice365ActivityService.IsOffice365UserLicenceChangingInProgress(username);
            if (isServiceChangingInProgress) return UserProvisioningStatus.InProgress;

            var isServiceRemovalInProgress = _workflowOffice365ActivityService.IsOffice365UserLicenceRemovalInProgress(username);
            if (isServiceRemovalInProgress) return UserProvisioningStatus.InProgress;

            var isServiceRestoreInProgress = _workflowOffice365ActivityService.IsOffice365UserLicenceRestoreInProgress(username);
            if (isServiceRestoreInProgress) return UserProvisioningStatus.InProgress;

            var isRolesChangingInProgress = _workflowOffice365ActivityService.IsOffice365UserRolesChangingInProgress(username);
            if (isRolesChangingInProgress) return UserProvisioningStatus.InProgress;

            var office365UserState = await _office365UserService.GetOffice365UserState(username);
            switch (office365UserState)
            {
                case Office365UserState.Active:
                    return UserProvisioningStatus.Assigned;
                case Office365UserState.Inactive:
                    return UserProvisioningStatus.Removed;
            }

            var domain = username.Split('@')[1];
            var isDomainVerified = await _office365DomainService.IsDomainVerified(domain);

            return isDomainVerified ? UserProvisioningStatus.Available : UserProvisioningStatus.NotAvailable;
        }
    }
}
