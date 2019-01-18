using CloudPlus.Database;
using CloudPlus.Exceptions.Catalog;
using CloudPlus.Exceptions.Company;
using CloudPlus.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using CloudPlus.Entities.Catalog;
using CloudPlus.Models.Catalog;
using CloudPlus.Entities;

namespace CloudPlus.Services.Database.Support
{
    public class CustomSecureControlPanelService : ICustomSecureControlPanelService
    {
        private readonly CldpDbContext _dbContext;
        public CustomSecureControlPanelService(
            CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CloudPlus.Entities.Company> CreateCustommeSecureControlPanel(CustomSecureControlPanelModel model)
        {
            var IsComapanyExist = _dbContext.Companies.Where(x => x.Id == model.Id && x.IsDeleted == false).FirstOrDefault();
            if (IsComapanyExist != null)
            {
                var IsControlPanelAlreadyExist= _dbContext.CustomSecureControlPanel.Where(x => x.CompanyID == model.Id && x.IsDeleted == false).FirstOrDefault();
                {
                    var ControlPanle = new CustomSecureControlPanel
                    {
                        CompanyID = model.Id,
                        CompanyName = model.CompanyName,
                        ContactPerson = model.ContactPerson,
                        ContactPhone = model.ContactPhone,
                        Email = model.Email,
                        CustomSecureControlPanelURL = model.CustomSecureControlPanelURL,
                        CompanyAddressStreet = model.CompanyAddressStreet,
                        CompanyAddressCity = model.CompanyAddressCity,
                        CompanyAddressState = model.CompanyAddressState,
                        CompanyAddressCountry = model.CompanyAddressCountry,
                        CompanyAddressZipCode = model.CompanyAddressZipCode,
                        StatusId = 1
                    };

                    _dbContext.CustomSecureControlPanel.Add(ControlPanle);

                    // var ModelComp = _dbContext.Companies.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (IsComapanyExist.ControlPanelSiteUrl != null)
                        IsComapanyExist.ControlPanelSiteUrl = model.CustomSecureControlPanelURL;
                    else
                        throw new Exception("Error there is no resseller data not saved!");

                    _dbContext.SaveChanges();

                }
                
            }
            else
            {
                throw new Exception("Error there is no resseller in this company id!");
            }
            return IsComapanyExist;

        }
        public async Task<int> GetProvisioningStatus(int companyId)
        {
            var provision = _dbContext.CustomSecureControlPanel.FirstOrDefault(p => p.CompanyID == companyId && p.IsDeleted == false);
            var ProvisionStatus = provision != null ? 1 : 0;
            return ProvisionStatus;
        }

        public CustomSecureControlPanelModel GetCustomSecurePanel(int companyId)
        {
            var customSecureControlPanel = _dbContext.CustomSecureControlPanel.FirstOrDefault(p => p.CompanyID == companyId && p.IsDeleted == false);
            var CustomSecureControlPanel = new CustomSecureControlPanelModel()
            {
                Id = customSecureControlPanel.Id,
                CompanyName = customSecureControlPanel.CompanyName,
                CompanyAddressCity = customSecureControlPanel.CompanyAddressCity,
                CompanyAddressStreet = customSecureControlPanel.CompanyAddressStreet,
                CompanyAddressState = customSecureControlPanel.CompanyAddressState,
                CompanyAddressCountry = customSecureControlPanel.CompanyAddressCountry,
                CompanyAddressZipCode = customSecureControlPanel.CompanyAddressZipCode,
                ContactPerson = customSecureControlPanel.ContactPerson,
                ContactPhone = customSecureControlPanel.ContactPhone,
                CustomSecureControlPanelURL = customSecureControlPanel.CustomSecureControlPanelURL,
            };
            return CustomSecureControlPanel;
        }

        public async Task<List<SupportModel>> GetSupportProducts(int companyId)
        {

            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems.Select(cp => cp.ProductItem.Product)))
                .FirstOrDefaultAsync(c => c.Id == companyId);


            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");
            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);


            if (assignedCatalog == null)
                throw new NoAssignedCatalogException($"CompanyId: {companyId}");

           // var productItems = assignedCatalog.Catalog.CatalogProductItems.Where(p => p.Available).Select(p => new CustomerProductItemModel
            var productItems = assignedCatalog.Catalog.CatalogProductItems.Select(p => new CustomerProductItemModel
            {
                ProductId = p.ProductItem.Product.Id,
                ProductItemId = p.ProductItemId,
                Name = p.ProductItem.Name,
                Description = p.ProductItem.Description,
                IsAddon = p.ProductItem.IsAddon,
                Identifier = p.ProductItem.Identifier,
                Cost = company.Type == 1 ? p.RetailPrice : p.ResellerPrice
            }).Where(x=>x.Name== "Custom Control Panel URL").ToList();
            //.Where(x => x.ProductId == 2).ToList();
            //productItems = productItems.Where(x => x.ProductId == 2).ToList();

            var distinctProducts = assignedCatalog.Catalog.CatalogProductItems.Select(p => p.ProductItem.Product).FirstOrDefault(x => x.Name == "(#CustomerName#) Support");
            //var distinctProducts = assignedCatalog.Catalog.CatalogProductItems.Select(p => p.ProductItem.Product).FirstOrDefault(x => x.Id == 2);
            //var SupportProduct = distinctProducts.FirstOrDefault(x => x.Id == 2);

            List<SupportModel> lstSupportModel = new List<SupportModel>();

            var CustomControlpanel = _dbContext.CustomSecureControlPanel.FirstOrDefault(x => x.CompanyID == companyId);
            if (CustomControlpanel != null)
            {
                if (CustomControlpanel.CustomSecureControlPanelURL != null)
                {

                    lstSupportModel = productItems.Select(Items => new SupportModel
                    {
                        activatedDate = CustomControlpanel.CreateDate,
                        imgUrl = distinctProducts.ImgUrl,
                        id = Items.ProductItemId,
                        name = Items.Name,
                        CustomControlPanel = _dbContext.CustomSecureControlPanel.Include(cs => cs.StatusId).Where(x => x.CompanyID == companyId).Select(x => new CustomControlPanelModel
                        {
                            urls = x.CustomSecureControlPanelURL,
                            Status = new Status
                            {
                                statusId = x.Status.Id,
                                statusValue = x.Status.Status,
                                statusIcon = x.Status.StatusIcon
                            }
                        }).ToList(),
                    }).ToList();
                }
            }
            return lstSupportModel;
        }
    }
}
