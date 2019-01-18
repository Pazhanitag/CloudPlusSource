using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Constants;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Database.Product;

namespace CloudPlus.Api.Controllers.Product
{
    [Authorize]
    [RoutePrefix("api/products")]
    public class ProductController : ApiController
    {
        private readonly IProductService _productServiceV2;
        private readonly ICustomerCatalogService _customerCatalogService;
        public ProductController(
            IProductService productServiceV2,
            ICustomerCatalogService customerCatalogService)
        {
            _productServiceV2 = productServiceV2;
            _customerCatalogService = customerCatalogService;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		[Route("", Name = "GetAllProductsByCompanyId")]
        public async Task<IHttpActionResult> Get()
        {
            var products = await _customerCatalogService.GetCustomerProducts(User.CompanyId());
            return Ok(products.Select(p => p.ToCustomerCatalogProductViewModel($"{Url.Content("~/")}Static/Images/Office365Icons")));
        }
        
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		[Route("{productId:int}", Name = "GetProductById")]
        public async Task<IHttpActionResult> Get(int productId)
        {
            var product = await _customerCatalogService.GetCustomerProduct(User.CompanyId(), productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.ToCustomerCatalogProductViewModel($"{Url.Content("~/")}Static/Images/Office365Icons"));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		[Route("category/{productId:int}", Name = "GetByCategoryId")]
        public async Task<IHttpActionResult> GetByCategory(int productId)
        {
            var product = await _productServiceV2.GetProduct(productId);

            if (product == null)
            {
                return NotFound();
            }

            var productsInViewModel = product.ToProductViewModel(Url.Content("~/"));
            return Ok(productsInViewModel);
        }
    }
}
