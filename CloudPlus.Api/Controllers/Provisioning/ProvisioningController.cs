using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Constants;
using CloudPlus.Enums.Provisions;
using CloudPlus.Services.Database.Provisions;

namespace CloudPlus.Api.Controllers.Provisioning
{
    [Authorize]
    [RoutePrefix("api/provisioning")]
    public class ProvisioningController : ApiController
    {
        private readonly IProvisioningService _provisioningService;

        public ProvisioningController(IProvisioningService provisioningService)
        {
            _provisioningService = provisioningService;
        }

        [Route("{companyId:int}/{productId:int}")]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public async Task<IHttpActionResult> Get(int companyId, int productId)
        {
            return Ok(await _provisioningService.GetComapnyProvisionedStatusAsync(companyId, productId));
        }

        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("{companyId:int}/{productId:int}")]
        public async Task<IHttpActionResult> Post(int companyId, int productId)
        {
            await _provisioningService.ProvisionAsync(companyId, productId);
            return Ok(await _provisioningService.GetComapnyProvisionedStatusAsync(companyId, productId));
        }

        [HttpPut]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("{companyId:int}/{productId:int}/{status:int}")]
        public async Task<IHttpActionResult> Put(int companyId, int productId, CompanyProvisioningStatus status)
        {
            return Ok(await _provisioningService.UpdateStatusAsync(companyId, productId, status));
        }

        [HttpDelete]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        [Route("{companyId:int}/{productId:int}")]
        public async Task<IHttpActionResult> Delete(int companyId, int productId)
        {
            return Ok(await _provisioningService.DeProvisionAsync(companyId, productId));
        }
    }
}