using System;
using System.Collections.Generic;
using CloudPlus.Database;
using System.Linq;
using CloudPlus.Models.Company;
using System.Data.Entity;
using System.Threading.Tasks;
using CloudPlus.Entities.Catalog;
using CloudPlus.Logging;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Services.Database.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly CldpDbContext _dbContext;
        private readonly IUserService _userService;
        public CompanyService(
            CldpDbContext dbContext,
            IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public bool IsMemberInCompanyHierarchy(int parentCompanyId, int childCompanyId)
        {
            var parentCompany = _dbContext.Companies.Include(c => c.MyCompanies)
                .FirstOrDefault(c => c.Id == parentCompanyId);

            if (parentCompany == null)
                throw new Exception($"There is no company with id {parentCompanyId}");

            var isMember = parentCompany.MyCompanies.Any(c => c.Id == childCompanyId);

            if (isMember)
                return true;

            foreach (var myCompany in parentCompany.MyCompanies)
            {
                isMember = IsMemberInCompanyHierarchy(myCompany.Id, childCompanyId);

                if (isMember)
                    break;
            }

            return isMember;
        }


        public async Task<CompanyModel> GetCompanyAsync(int companyId)
        {
            var company = await _dbContext.Companies
                .Include(c => c.Domains)
                .Include(c => c.CompanyCatalogs)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null) return null;

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned);

            return new CompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                CompanyOu = company.CompanyOu,
                ParentId = company.ParentId,
                Status = (CompanyStatus)company.Status,
                Type = (CompanyType)company.Type,
                UniqueIdentifier = company.UniqueIdentifier,
                StreetAddress = company.StreetAddress,
                BrandColorPrimary = company.BrandColorPrimary,
                Email = company.Email,
                City = company.City,
                Country = company.Country,
                State = company.State,
                ZipCode = company.ZipCode,
                PhoneNumber = company.PhoneNumber,
                LogoUrl = company.LogoUrl,
                BrandColorSecondary = company.BrandColorSecondary,
                BrandColorText = company.BrandColorText,
                Website = company.Website,
                SupportSiteUrl = company.SupportSiteUrl,
                ControlPanelSiteUrl = company.ControlPanelSiteUrl,
                CreateDate = company.CreateDate,
                CatalogId = assignedCatalog?.CatalogId,
                Domains = company.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })

            };
        }

        public CompanyModel GetCompany(int companyId)
        {
            var company = _dbContext.Companies.Include(c => c.Domains).FirstOrDefault(c => c.Id == companyId);

            return company != null ? new CompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                CompanyOu = company.CompanyOu,
                ParentId = company.ParentId,
                Status = (CompanyStatus)company.Status,
                Type = (CompanyType)company.Type,
                UniqueIdentifier = company.UniqueIdentifier,
                StreetAddress = company.StreetAddress,
                BrandColorPrimary = company.BrandColorPrimary,
                Email = company.Email,
                City = company.City,
                Country = company.Country,
                State = company.State,
                ZipCode = company.ZipCode,
                PhoneNumber = company.PhoneNumber,
                LogoUrl = company.LogoUrl,
                BrandColorSecondary = company.BrandColorSecondary,
                BrandColorText = company.BrandColorText,
                Website = company.Website,
                SupportSiteUrl = company.SupportSiteUrl,
                ControlPanelSiteUrl = company.ControlPanelSiteUrl,
                CreateDate = company.CreateDate,
                Domains = company.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })

            } : null;
        }

        public CompanyModel GetCompany(string uniqueIdentifier)
        {
            var company = _dbContext.Companies.Include(c => c.Domains).FirstOrDefault(c => c.UniqueIdentifier == uniqueIdentifier);
            return ToCompanyModel(company);
        }

        public async Task<CompanyModel> GetCompanyParentAsync(int companyId)
        {
            var childCompany = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (childCompany == null) return null;

            var company = await _dbContext.Companies.Include(c => c.Domains).FirstOrDefaultAsync(c => c.Id == childCompany.ParentId);

            return company != null ? new CompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                CompanyOu = company.CompanyOu,
                ParentId = company.ParentId,
                Status = (CompanyStatus)company.Status,
                Type = (CompanyType)company.Type,
                UniqueIdentifier = company.UniqueIdentifier,
                StreetAddress = company.StreetAddress,
                BrandColorPrimary = company.BrandColorPrimary,
                Email = company.Email,
                City = company.City,
                Country = company.Country,
                State = company.State,
                ZipCode = company.ZipCode,
                PhoneNumber = company.PhoneNumber,
                LogoUrl = company.LogoUrl,
                BrandColorSecondary = company.BrandColorSecondary,
                BrandColorText = company.BrandColorText,
                Website = company.Website,
                SupportSiteUrl = company.SupportSiteUrl,
                ControlPanelSiteUrl = company.ControlPanelSiteUrl,
                CreateDate = company.CreateDate,
                Domains = company.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })
            } : null;
        }

        public IEnumerable<CompanyModel> GetCompanies(int? parentCompanyId, CompanyType companyType)
        {
            var companies = _dbContext.Companies
                .Where(c => c.ParentId == parentCompanyId && c.Type == (int)companyType);

            return companies.Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
                CompanyOu = c.CompanyOu,
                ParentId = c.ParentId,
                Status = (CompanyStatus)c.Status,
                Type = (CompanyType)c.Type,
                UniqueIdentifier = c.UniqueIdentifier,
                StreetAddress = c.StreetAddress,
                BrandColorPrimary = c.BrandColorPrimary,
                Email = c.Email,
                City = c.City,
                Country = c.Country,
                State = c.State,
                ZipCode = c.ZipCode,
                PhoneNumber = c.PhoneNumber,
                LogoUrl = c.LogoUrl,
                BrandColorSecondary = c.BrandColorSecondary,
                BrandColorText = c.BrandColorText,
                Website = c.Website,
                CreateDate = c.CreateDate,
                Domains = c.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })
            });
        }

        public async Task<(int resellersCount, int customersCount, int usersCount, int resellersDirectChildrenCount, int customersDirectChildrenCount)> GetCompanyHierarchyCount(int companyId)
        {
            var company = await _dbContext.Companies
                .Include(c => c.MyCompanies)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new Exception($"There is no company with id {companyId}");

            var resellersCount = company.MyCompanies.Count(c => c.Type == (int)CompanyType.Reseller);
            var customersCount = company.MyCompanies.Count(c => c.Type == (int)CompanyType.Customer);

            foreach (var myCompany in company.MyCompanies)
            {
                var details = await GetCompanyHierarchyCount(myCompany.Id);

                resellersCount += details.resellersCount;
                customersCount += details.customersCount;
            }

            var usersCount = _userService.GetUsers(companyId).Count();
            var resellersDirectChildrenCount = company.MyCompanies.Count(c => c.Type == (int)CompanyType.Reseller);
            var customersDirectChildrenCount = company.MyCompanies.Count(c => c.Type == (int)CompanyType.Customer);
            return (resellersCount: resellersCount, customersCount: customersCount, usersCount: usersCount, resellersDirectChildrenCount: resellersDirectChildrenCount, customersDirectChildrenCount: customersDirectChildrenCount);
        }

        public async Task<CompanyModel> CreateCompany(CompanyModel company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            try
            {
                var dbCompany = new Entities.Company
                {
                    CompanyOu = company.CompanyOu,
                    Name = company.Name,
                    Status = Convert.ToInt32(company.Status),
                    Type = Convert.ToInt32(company.Type),
                    UniqueIdentifier = Guid.NewGuid().ToString(),
                    ParentId = company.ParentId,
                    BrandColorPrimary = company.BrandColorPrimary,
                    BrandColorSecondary = company.BrandColorSecondary,
                    BrandColorText = company.BrandColorText,
                    City = company.City,
                    Country = company.Country,
                    Email = company.Email,
                    LogoUrl = company.LogoUrl,
                    PhoneNumber = company.PhoneNumber,
                    State = company.State,
                    StreetAddress = company.StreetAddress,
                    Website = company.Website,
                    ZipCode = company.ZipCode,
                    SupportSiteUrl = company.SupportSiteUrl,
                    ControlPanelSiteUrl = company.ControlPanelSiteUrl
                };

                _dbContext.Companies.Add(dbCompany);

                var domains = company.Domains.Select(d => new Entities.Domain
                {
                    Company = dbCompany,
                    IsPrimary = d.IsPrimary,
                    Name = d.Name.ToLower()
                });

                _dbContext.Domains.AddRange(domains);

                await _dbContext.SaveChangesAsync();

                company.Id = dbCompany.Id;

                //mapping company with the default vendor metrics
                if (company?.Id != null && company.Id != 0)
                {
                    SetCompanyMetrics(company.Id);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Failed to create new company", ex);
                throw;
            }

            return company;
        }

        public bool SetCompanyMetrics(int companyId)
        {
            try
            {
                var Metriclist = _dbContext.VendorMetrics.Where(x => x.IsDeleted == false).ToList();

                if (Metriclist?.Count() > 0)
                {
                    foreach (var metric in Metriclist)
                    {
                        var companyMetric = _dbContext.VendorMetricsAdminConfig.Where(x => x.MetricsId == metric.Id && x.CompanyId == companyId && x.IsDeleted == false).FirstOrDefault();
                        if (companyMetric != null)
                        {
                            companyMetric.CanAccess = metric.CanAccess;
                            companyMetric.UpdateDate = DateTime.Now;
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            companyMetric = new Entities.VendorMetricsAdminConfig();
                            companyMetric.CanAccess = metric.CanAccess;
                            companyMetric.CompanyId = companyId;
                            companyMetric.CreateDate = DateTime.Now;
                            companyMetric.IsDeleted = false;
                            companyMetric.MetricsId = metric.Id;
                            _dbContext.Entry(companyMetric).State = EntityState.Added;
                            _dbContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    this.Log().Info("No vendor metrics details available to map with the new company");
                    return false;
                }

            }
            catch (Exception ex)
            {
                this.Log().Error("Failed to map the vendor metrics to the new company", ex);
            }
            this.Log().Info("Successfully mapped the vendor metrics dashboard details for the new company");
            return true;
        }

        public void DeleteCompany(int id)
        {
            var company = _dbContext.Companies.FirstOrDefault(c => c.Id == id);

            if (company == null) return;

            _dbContext.Companies.Remove(company);
            _dbContext.SaveChanges();
        }

        public async Task<CompanyModel> UpdateAsync(CompanyModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                var dbCompany = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == model.Id);
                if (dbCompany == null)
                    throw new NullReferenceException(nameof(dbCompany));

                ModelToEntity(model, dbCompany);

                if (model.Domains.Any())
                {
                    foreach (var domain in model.Domains)
                    {
                        _dbContext.Domains.Add(new Entities.Domain
                        {
                            CompanyId = model.Id,
                            Name = domain.Name.ToLower(),
                            IsPrimary = domain.IsPrimary
                        });
                    }
                }

                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                this.Log().Error("Failed to update company", ex);
                throw;
            }
        }

        private void ModelToEntity(CompanyModel model, Entities.Company company)
        {
            company.BrandColorPrimary = model.BrandColorPrimary;
            company.BrandColorSecondary = model.BrandColorSecondary;
            company.BrandColorText = model.BrandColorText;
            company.City = model.City;
            company.Country = model.Country;
            company.Email = model.Email;
            company.Name = model.Name;
            company.PhoneNumber = model.PhoneNumber;
            company.State = model.State;
            company.StreetAddress = model.StreetAddress;
            company.SupportSiteUrl = model.SupportSiteUrl;
            company.ControlPanelSiteUrl = model.ControlPanelSiteUrl;
            company.Website = model.Website;
            company.ZipCode = model.ZipCode;
            if (!string.IsNullOrWhiteSpace(model.LogoUrl))
                company.LogoUrl = model.LogoUrl;
        }

        private CompanyModel ToCompanyModel(Entities.Company company)
        {
            return company != null ? new CompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                CompanyOu = company.CompanyOu,
                ParentId = company.ParentId,
                Status = (CompanyStatus)company.Status,
                Type = (CompanyType)company.Type,
                UniqueIdentifier = company.UniqueIdentifier,
                StreetAddress = company.StreetAddress,
                BrandColorPrimary = company.BrandColorPrimary,
                Email = company.Email,
                City = company.City,
                Country = company.Country,
                State = company.State,
                ZipCode = company.ZipCode,
                PhoneNumber = company.PhoneNumber,
                LogoUrl = company.LogoUrl,
                BrandColorSecondary = company.BrandColorSecondary,
                BrandColorText = company.BrandColorText,
                Website = company.Website,
                SupportSiteUrl = company.SupportSiteUrl,
                ControlPanelSiteUrl = company.ControlPanelSiteUrl,
                CreateDate = company.CreateDate,
                Domains = company.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })

            } : null;
        }

        public IEnumerable<CompanyModel> GetAllCompanies()
        {
            var companies = _dbContext.Companies;

            return companies.Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
                CompanyOu = c.CompanyOu,
                ParentId = c.ParentId,
                Status = (CompanyStatus)c.Status,
                Type = (CompanyType)c.Type,
                UniqueIdentifier = c.UniqueIdentifier,
                StreetAddress = c.StreetAddress,
                BrandColorPrimary = c.BrandColorPrimary,
                Email = c.Email,
                City = c.City,
                Country = c.Country,
                State = c.State,
                ZipCode = c.ZipCode,
                PhoneNumber = c.PhoneNumber,
                LogoUrl = c.LogoUrl,
                BrandColorSecondary = c.BrandColorSecondary,
                BrandColorText = c.BrandColorText,
                Website = c.Website,
                CreateDate = c.CreateDate,
                Domains = c.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })
            });
        }

        List<CompanyModel> GetCompanyList(IEnumerable<Entities.Company> lstEntityCompanies)
        {
            List<CompanyModel> lstCompanyModel = new List<CompanyModel>();
            lstCompanyModel = lstEntityCompanies.Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
                CompanyOu = c.CompanyOu,
                ParentId = c.ParentId,
                Status = (CompanyStatus)c.Status,
                Type = (CompanyType)c.Type,
                UniqueIdentifier = c.UniqueIdentifier,
                StreetAddress = c.StreetAddress,
                BrandColorPrimary = c.BrandColorPrimary,
                Email = c.Email,
                City = c.City,
                Country = c.Country,
                State = c.State,
                ZipCode = c.ZipCode,
                PhoneNumber = c.PhoneNumber,
                LogoUrl = c.LogoUrl,
                BrandColorSecondary = c.BrandColorSecondary,
                BrandColorText = c.BrandColorText,
                Website = c.Website,
                CreateDate = c.CreateDate,
                Domains = c.Domains.Select(d => new DomainModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsPrimary = d.IsPrimary
                })
            }).ToList();
            return lstCompanyModel;
        }

        public IEnumerable<CompanyModel> GetCompaniesbyFilter(int? parentCompanyId, CompanyType companyType, string SearchKey = "")
        {
            List<Entities.Company> companyList = new List<Entities.Company>();
            GetRecursiveFilterOnEnity(parentCompanyId, SearchKey, companyType, ref companyList);
            companyList = companyList.AsEnumerable().Where(c=>c.Type.Equals((int)companyType)).ToList();
            List<CompanyModel> lstCompanyModel = GetCompanyList(companyList);
            return lstCompanyModel;
        }
        public void GetRecursiveFilterOnEnity(int? parentCompanyId, String SearchKey, CompanyType companyType, ref List<Entities.Company> companyList)
        {

            var company = _dbContext.Companies
                .Include(x => x.MyCompanies).Include(y => y.Domains)
                .Where(c => c.Id == parentCompanyId).FirstOrDefault();
            if (company?.MyCompanies != null)
            {
                var AllCompanies = company.MyCompanies;
                companyList.AddRange(AllCompanies.Where(c => c.Name.ToLower().Contains(SearchKey.ToLower()) || c.Id.ToString().Contains(SearchKey)
                                                    || c.Country.ToLower().Contains(SearchKey.ToLower())
                                                    || c.State.ToLower().Contains(SearchKey.ToLower()) || c.City.ToLower().Contains(SearchKey.ToLower())
                                                    || c.Email.Contains(SearchKey) || c.PhoneNumber.Contains(SearchKey)
                                                    || c.CreateDate.ToString().Contains(SearchKey) || c.Domains.Any(x => x.Name.ToLower().Contains(SearchKey.ToLower()))).ToList());
                foreach (var com in AllCompanies)
                {
                    GetRecursiveFilterOnEnity(com.Id, SearchKey, companyType, ref companyList);
                }
            }
        }
    }
}
