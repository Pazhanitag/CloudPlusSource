using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Helpers;
using CloudPlus.Constants;
using CloudPlus.Services.Database.Catalog;

namespace CloudPlus.Api.Controllers.Utilities
{
    [RoutePrefix("api/rpc/catalogutilities")]
    public class CatalogUtilitiesController : ApiController
    {
        private readonly ICompanyCatalogService _companyCatalogService;
        private readonly ICatalogHelper _catalogHelper;

        public CatalogUtilitiesController(ICompanyCatalogService companyCatalogService, ICatalogHelper catalogHelper)
        {
            _companyCatalogService = companyCatalogService;
            _catalogHelper = catalogHelper;
        }
        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
		[Route("changedefaultresellercatalog/{catalogId:int}", Name = "ChangeDefaultResellerCatalog")]
        public async Task<IHttpActionResult> ChangeDefaultResellerCatalog(int catalogId)
        {
            await _companyCatalogService.ChangeDefaultCompanyCatalog(User.CompanyId(), catalogId);
            return Ok(await _catalogHelper.GetCatalogs(User.CompanyId()));
        }
    }
}