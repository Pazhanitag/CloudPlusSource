using CloudPlus.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Services.Database.Support
{
    public interface ICustomSecureControlPanelService
    {
       Task<CloudPlus.Entities.Company> CreateCustommeSecureControlPanel(CustomSecureControlPanelModel objCustomSecureControlPanelModel);
        Task<int> GetProvisioningStatus(int CompanyID);
        CustomSecureControlPanelModel GetCustomSecurePanel(int CompanyID);
        Task<List<SupportModel>> GetSupportProducts(int CompanyId);
    }
}
