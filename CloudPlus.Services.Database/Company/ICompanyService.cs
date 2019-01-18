using CloudPlus.Models.Company;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Enums;

namespace CloudPlus.Services.Database.Company
{
    public interface ICompanyService
    {
        bool IsMemberInCompanyHierarchy(int parentId, int childId);
        Task<CompanyModel> GetCompanyAsync(int companyId);
        CompanyModel GetCompany(int companyId);
	    CompanyModel GetCompany(string uniqueIdentifier);
		Task<CompanyModel> GetCompanyParentAsync(int companyId);
        void DeleteCompany(int id);
        Task<CompanyModel> CreateCompany(CompanyModel company);
        IEnumerable<CompanyModel> GetCompanies(int? parentCompanyId, CompanyType companyType);
        Task<CompanyModel> UpdateAsync(CompanyModel company);
        // At this point these are only details of the company that are not in a model so we can just name a function like this
        Task<(int resellersCount, int customersCount, int usersCount, int resellersDirectChildrenCount, int customersDirectChildrenCount)> GetCompanyHierarchyCount(int companyId);
        /// <summary>
        /// Added by TAG to get all the list of companies for the configuring the dashboard widgets
        /// </summary>
        /// <returns></returns>
        IEnumerable<CompanyModel> GetAllCompanies();
        IEnumerable<CompanyModel> GetCompaniesbyFilter(int? parentCompanyId, CompanyType companyType, string Search);
        
    }
}
