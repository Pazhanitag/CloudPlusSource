using System;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Mappers;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Identity.User;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.ViewModels.Request.Company;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.Companies.Commands;
using System.Net;
using CloudPlus.Services.Identity.Client;

namespace CloudPlus.Api.Controllers.Utilities
{
	[Authorize]
	[RoutePrefix("api/rpc/companyutilities")]
    public class CompanyUtilitiesController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
	    private readonly IMessageBroker _messageBroker;
	    private readonly IImageHelper _imageHelper;
	    private readonly IClientService _clientService;
	    
		public CompanyUtilitiesController(ICompanyService companyService,
			IUserService userService,
			IMessageBroker messageBroker,
			IImageHelper imageHelper,
			IClientService clientService)
        {
            _companyService = companyService;
            _userService = userService;
	        _messageBroker = messageBroker;
	        _imageHelper = imageHelper;
	        _clientService = clientService;
        }

        [HttpGet]
        [Route("getbrandingforusercompany")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBrandingForCompanyByUserEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var user = await _userService.GetUserAsync(email);

            if (user == null)
                return NotFound();

            var company = await _companyService.GetCompanyAsync(user.CompanyId);

            if (company == null)
                return NotFound();

            return Ok(company.ToBrandingViewModel(Url.Content("~/")));
        }

	    [HttpPost]
		[ValidateModel]
		[AllowAnonymous]
		[Route("createcompany")]
	    public async Task<IHttpActionResult> Create(CreateCompanyViewModel model)
	    {
		    if (model.ParentId == 0)
		    {
			    if (String.IsNullOrEmpty(model.ParentUniqueIdentifier))
			    {
				    return StatusCode(HttpStatusCode.Forbidden);
			    }

			    var company = _companyService.GetCompany(model.ParentUniqueIdentifier);
			    model.Logo = _imageHelper.SaveCompanyLogo($"prepopulatedPicture{company.LogoUrl}");
			    model.ParentId = company.Id;
		    }
                
		    var createCompanyQueue = CompanyServiceConstants.QueueCreateCompany;

		    model.ClientDbId = await _clientService.GetClientDbId("cloudplusAdminPortal");
            await _messageBroker.GetSendEndpoint(createCompanyQueue)
                .Send<ICreateCompanyCommand>(
                    model.ToCreateCompanyCommand()
                );

            return Ok();
	    }
    }
}
