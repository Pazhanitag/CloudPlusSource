using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.Services.Database.Office365.Domain;
using CloudPlus.Services.Database.WorkflowActivity.Office365;

namespace CloudPlus.Api.Controllers.Office365
{
    [Authorize]
    [RoutePrefix("api/Office365Domains")]
    public class Office365DomainController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IWorkflowOffice365ActivityService _workflowOffice365ActivityService;
        private readonly IOffice365DbDomainService _office365DomainService;

        public Office365DomainController(
            IMessageBroker messageBroker,
            IWorkflowOffice365ActivityService workflowOffice365ActivityService,
            IOffice365DbDomainService office365DomainService)
        {
            _messageBroker = messageBroker;
            _workflowOffice365ActivityService = workflowOffice365ActivityService;
            _office365DomainService = office365DomainService;
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		[Route(Name = "AddAdditionalDomain")]
        public async Task<IHttpActionResult> AddAdditionalDomain([FromBody]Office365AddAdditionalDomainViewModel model)
        {
            if (User.CompanyId() != model.CompanyId)
                return NotFound();

            if (_workflowOffice365ActivityService.IsOffice365AddingAdditionalDomainInProgress(model.Domain))
                return Conflict();

            await _messageBroker.GetSendEndpoint(Office365ServiceConstants.QueueAddAdditionalOffice365Domain)
                .Send<IOffice365AddAdditionalDomainCommand>(model.ToOffice365AddAdditionalDomainCommand());

            return Ok();
        }

        [HttpGet]
        [Route("IsDomainFederated")]
        public async Task<IHttpActionResult> IsDomainFederated(string domain)
        {
            var isFederated = await _office365DomainService.IsDomainFederated(domain);
            return Ok(isFederated);
        }
    }
}
