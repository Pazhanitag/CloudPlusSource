using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.QueueModels.Office365.Customer.Commands;

namespace CloudPlus.Api.Controllers.Office365
{
    [Authorize]
    [RoutePrefix("api/office365customers")]
    public class Office365CustomerController : ApiController
    {
        private readonly IMessageBroker _messageBroker;

        public Office365CustomerController(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        [HttpPost]
        [ValidateModel]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		[Route(Name = "CreateOffice365Customer")]
        public async Task<IHttpActionResult> CreateOffice365Customer([FromBody]Office365CreateCustomerViewModel model)
        {
            if (User.CompanyId() != model.CompanyId)
                return NotFound();

            var createOffice365CustomerQueue = Office365ServiceConstants.QueueCreateOffice365Customer;

            await _messageBroker.GetSendEndpoint(createOffice365CustomerQueue)
                .Send<IOffice365CreateCustommerCommand>(model.ToOffice365CreateCustomerCommand());

            return Ok();
        }
    }
}
