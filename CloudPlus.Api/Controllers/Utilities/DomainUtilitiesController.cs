using System;
using System.Web.Http;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.WorkflowActivity.Company;

namespace CloudPlus.Api.Controllers.Utilities
{
    [Authorize]
    [RoutePrefix("api/rpc/domainutilities")]
    public class DomainUtilitiesController : ApiController
    {
        private readonly IWorkflowCompanyActivityService _workflowCompanyActivityService;
        private readonly IDomainService _domainService;

        public DomainUtilitiesController(IWorkflowCompanyActivityService workflowCompanyActivityService, IDomainService domainService)
        {
            _workflowCompanyActivityService = workflowCompanyActivityService;
            _domainService = domainService;
        }
        [HttpGet]
        [Route("domainAvailable")]
		[AllowAnonymous]
        public IHttpActionResult DomainAvailable(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(name)} is null or empty");

            var existingDomain = _domainService.GetDomainByName(name);

            if (existingDomain != null || _workflowCompanyActivityService.IsDomainBeingCreated(name))
                return Ok(false);

            return Ok(true);
        }
    }
}
