using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Utilities;
using System.Collections.Generic;

namespace CloudPlus.Services.Database.Office365.Utilities
{
    public interface IOffice365DbUtilitiesService
    {
        Office365ProvisioningStatus CheckProvisioningStatus(int companyId);
        Office365CompatibleMatrixModel GetOffice365CompatibleMatrix();
        bool AddUpdateCompatibleMatrix(List<Office365CompatabileMatrix> Office365CompatabileMatrix);
    }
}
