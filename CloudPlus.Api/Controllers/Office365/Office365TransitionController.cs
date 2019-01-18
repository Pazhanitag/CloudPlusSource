using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Transition;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Models.Office365.Transition;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Office365.Transition;

namespace CloudPlus.Api.Controllers.Office365
{
    [Authorize]
    [RoutePrefix("api/Office365Transitions")]
    public class Office365TransitionController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IOffice365TransitionService _office365TransitionService;
        private readonly IDomainService _domainService;

        public Office365TransitionController(
            IMessageBroker messageBroker,
            IOffice365TransitionService office365TransitionService,
            IDomainService domainService)
        {
            _messageBroker = messageBroker;
            _office365TransitionService = office365TransitionService;
            _domainService = domainService;
        }

        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [ValidateModel]
        [Route("Transition", Name = "Transition")]
        public async Task<IHttpActionResult> Transition([FromBody]Office365TransitionViewModel model)
        {
            var office365Transition = Office365ServiceConstants.QueueOffice365Transition;
            var domains = _domainService.GetCompanyDomains(model.CompanyId).ToList();
            var cleanedProductItems = new List<Office365TransitionProductItemModel>();

            foreach (var item in model.ProductItems)
            {
                var domain = item.UserPrincipalName.Split('@').ElementAtOrDefault(1);
                if (domain == null) continue;

                if (domains.Any(d => string.Equals(d.Name, domain, StringComparison.InvariantCultureIgnoreCase)))
                    cleanedProductItems.Add(item);
            }

            model.ProductItems = cleanedProductItems;

            await _messageBroker.GetSendEndpoint(office365Transition)
                .Send<IOffice365TransitionCommand>(model.ToOffice365UserRemoveLicenseCommand());

            return Ok();
        }

        [HttpGet]
        [ValidateModel]
        // TODO [AuthorizeAccess(Permissions = "AddUser")]
        [Route("CheckAuthorization", Name = "CheckAuthorization")]
        public async Task<IHttpActionResult> CheckAuthorization()
        {
            var companyId = User.CompanyId();
            var result = await _office365TransitionService.CompanyBelongsToCloudPlusOffice365Async(companyId);

            return Ok(result);
        }

        [HttpGet]
        [ValidateModel]
        // TODO [AuthorizeAccess(Permissions = "AddUser")]
        [Route("TransitionMatchingData", Name = "TransitionMatchingData")]
        public async Task<IHttpActionResult> TransitionMatchingData()
        {
            var companyId = User.CompanyId();
            var result = await _office365TransitionService.GetTransitionMatchingDataAsync(companyId);

            return Ok(result);
        }
    }
}
