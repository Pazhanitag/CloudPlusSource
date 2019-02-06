using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Company;
using CloudPlus.Api.ViewModels.Response.Paging;
using CloudPlus.Api.ViewModels.Response.User;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Logging;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Companies.Commands;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.Metrics;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Identity.Client;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Api.Controllers.Company
{
    [Authorize]
    [RoutePrefix("api/companies")]
    public class CompaniesController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ICompanyService _companyService;
        private readonly IDomainService _domainService;
        private readonly IUserService _userService;
        private readonly IImageHelper _imageHelper;
        private readonly IOffice365DbCustomerService _office365CompanyService;
        private readonly IClientService _clientService;
        private readonly IMetricsService _metricsService;

        public CompaniesController(
            IMessageBroker messageBroker,
            ICompanyService companyService,
            IDomainService domainService,
            IUserService userService,
            IImageHelper imageHelper,
            IOffice365DbCustomerService office365CompanyService, 
            IClientService clientService, IMetricsService metricsService)
        {
            _messageBroker = messageBroker;
            _companyService = companyService;
            _domainService = domainService;
            _userService = userService;
            _imageHelper = imageHelper;
            _office365CompanyService = office365CompanyService;
            _clientService = clientService;
            _metricsService = metricsService;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
		[Route("{companyId:int}/domains", Name = "GetDomains")]
        public IHttpActionResult GetDomains(int companyId)
        {
            return Ok(_domainService.GetCompanyDomains(companyId));
        }

        [HttpGet]
		[Route("branding", Name = "GetBranding")]
        public async Task<IHttpActionResult> GetBranding()
        {
            var company = await _companyService.GetCompanyAsync(User.CompanyId());
            if (company == null)
                return NotFound();
            return Ok(company.ToBrandingViewModel(Url.Content("~/")));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts)]
		public async Task<IHttpActionResult> Get()
        {
            this.Log().Info("=> Get Company");

            var company = await _companyService.GetCompanyAsync(User.CompanyId());

            if (company == null)
                return NotFound();

            return Ok(company.ToCompanyViewModel(Url.Content("~/")));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
		[Route("{id:int}", Name = "GetCompanyById")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var company = await _companyService.GetCompanyAsync(id);

            if (company == null)
                return NotFound();

            return Ok(company.ToCompanyViewModel(Url.Content("~/")));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers, PermissionsConstants.ViewMyCompany)]
		[Route("{id:int}/parentcompany", Name = "GetParentCompanyByCompanyId")]
        public async Task<IHttpActionResult> GetParentCompany(int id)
        {
            var company = await _companyService.GetCompanyParentAsync(id);

            if (company == null)
                return NotFound();

            return Ok(company.ToCompanyViewModel(Url.Content("~/")));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
		[Route("{companyId:int}/users", Name = "GetCompanyUsers")]
        public IHttpActionResult GetCompanyUsers(int companyId)
        {
            var parentId = User.CompanyId();
            if (parentId != companyId && !_companyService.IsMemberInCompanyHierarchy(parentId, companyId))
                return NotFound();

            var response = _userService.GetUsers(companyId).Select(u => u.ToUserViewModel(Url.Content("~/")));
            return Ok(response);
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
		[Route("{companyId:int}/users/{page:int}/{pageSize:int}/{orderByColumn}/{order}/{searchTerm?}", Name = "SearchCompanyUsers")]
        public IHttpActionResult SearchCompanyUsers(int companyId, int page, int pageSize, string orderByColumn, string order, string searchTerm = "")
        {
            var parentId = User.CompanyId();
            if (parentId != companyId && !_companyService.IsMemberInCompanyHierarchy(parentId, companyId))
                return NotFound();

            var users = _userService.SearchUsers(searchTerm, companyId);

            var pageResponse = new PagedResultViewModel<UserViewModel>(Request, users.Select(u => u.ToUserViewModel(Url.Content("~/"))).AsQueryable(),
                page, pageSize, orderByColumn, order, "SearchCompanyUsers");

            return pageResponse;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
		[Route("{companyId:int}/companies/{companyType:int?}", Name = "GetCompanyChildren")]
        public async Task<IHttpActionResult> GetCompanyChildren(int companyId, int companyType = 0)
        {
            if (User.CompanyId() != companyId)
                return NotFound();
            
            var companies = _companyService.GetCompanies(companyId, (CompanyType)companyType).ToList();
            
            foreach (var company in companies)
            {
                var companyData = await _companyService.GetCompanyHierarchyCount(company.Id);
                company.NumberOfResellers = companyData.resellersDirectChildrenCount;
                company.NumberOfCustomers = companyData.customersDirectChildrenCount;
                company.NumberOfUsers = companyData.usersCount;
                company.numberOfTotalResellers = companyData.resellersCount;
                company.numberOfTotalCustomers = companyData.customersCount;
            }

            return Ok(companies.Select(company => company.ToCompanyViewModel(Url.Content("~/")))); 
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
		[Route("{companyId}/office365domains", Name = "GetOffice365Domains")]
        public async Task<IHttpActionResult> GetOffice365Domains(int companyId)
        {
            if (User.CompanyId() != companyId)
                return NotFound();

            var office365Company = await _office365CompanyService.GetCompanyOffice365DomainsAsync(companyId);

            return Ok(office365Company.ToOffice365CompanyDomainsViewModel());
        }

        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.AddAccounts, PermissionsConstants.AddUsers)]
		[ValidateModel]
        [Route(Name = "CreateCompany")]
        public async Task<IHttpActionResult> CreateCompany(CreateCompanyViewModel model)
        {
            if (model.ParentId != User.CompanyId())
                return StatusCode(HttpStatusCode.Forbidden);

            var createCompanyQueue = CompanyServiceConstants.QueueCreateCompany;

            model.User.ProfilePicture = _imageHelper.SaveProfilePicture(model.User.AvatarBase64);
            model.Logo = _imageHelper.SaveCompanyLogo(model.LogoBase64);
            // TODO client id should be loaded from config
            model.ClientDbId = await _clientService.GetClientDbId("cloudplusAdminPortal");

            await _messageBroker.GetSendEndpoint(createCompanyQueue)
                .Send<ICreateCompanyCommand>(
                    model.ToCreateCompanyCommand()
                );
            
            //var result = _metricsService.SetCompanyMetrics(model.ClientDbId);
            return Ok();
        }

        [HttpPut]
        [AuthorizeAccess(PermissionsConstants.EditAccounts, PermissionsConstants.EditUsers)]
		[ValidateModel]
        [Route(Name = "UpdateCompany")]
        public async Task<IHttpActionResult> UpdateCompany(UpdateCompanyViewModel model)
        {
            if (User.CompanyId() == model.Id)
            {
                model.CatalogId = null;
            }
            model.Logo = _imageHelper.SaveCompanyLogo(model.LogoBase64);
            model.ParentId = User.CompanyId();
            var updateCompanyQueue = CompanyServiceConstants.QueueUpdateCompany;

            await _messageBroker.GetSendEndpoint(updateCompanyQueue)
                .Send<IUpdateCompanyCommand>(
                    model.ToUpdateCompanyCommand()
                );

            return Ok();
        }

        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
        [Route("AllCompanies", Name = "AllCompanies")]
        public IHttpActionResult GetAllCompanies()
        {           
            var companies = _companyService.GetAllCompanies().ToList();           
            return Ok(companies);
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewAccounts, PermissionsConstants.ViewUsers)]
        [Route("FilterCompaniesByType", Name = "FilterCompaniesByType")]
        public async Task<IHttpActionResult> FilterCompaniesByType(int companyId, int companyType = 0,string search="")
        {
            // var companyType = 0;
            var companies = _companyService.GetCompaniesbyFilter(companyId, (CompanyType)companyType, search).ToList();

            foreach (var company in companies)
            {
                var companyData = await _companyService.GetCompanyHierarchyCount(company.Id);
                company.NumberOfResellers = companyData.resellersDirectChildrenCount;
                company.NumberOfCustomers = companyData.customersDirectChildrenCount;
                company.NumberOfUsers = companyData.usersCount;
                company.numberOfTotalResellers = companyData.resellersCount;
                company.numberOfTotalCustomers = companyData.customersCount;
            }

            return Ok(companies.Select(company => company.ToCompanyViewModel(Url.Content("~/"))));
        }
       
    }
}
