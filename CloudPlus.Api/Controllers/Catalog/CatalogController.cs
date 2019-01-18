using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Catalog;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Models.Catalog;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.Services.Database.Catalog;


namespace CloudPlus.Api.Controllers.Catalog
{
    [RoutePrefix("api/catalogs")]
    public class CatalogController : ApiController
    {
        private readonly ICompanyCatalogService _companyCatalogService;
        private readonly ICatalogProductItemService _catalogProductItemService;
        private readonly ICustomerCatalogService _customerCatalogService;
        private readonly ICatalogHelper _catalogHelper;
        private readonly IMessageBroker _messageBroker;

        public CatalogController(
            ICompanyCatalogService companyCatalogService,
            ICatalogProductItemService catalogProductItemService,
            ICustomerCatalogService customerCatalogService,
            ICatalogHelper catalogHelper,
            IMessageBroker messageBroker)
        {
            _companyCatalogService = companyCatalogService;
            _catalogProductItemService = catalogProductItemService;
            _customerCatalogService = customerCatalogService;
            _catalogHelper = catalogHelper;
            _messageBroker = messageBroker;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("reseller", Name = "GetCatalogs")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _catalogHelper.GetCatalogs(User.CompanyId()));
        }
        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("reseller", Name = "CreateCatalog")]
        public async Task<IHttpActionResult> Create(CreateCatalogViewModel createCatalogviewModel)
        {
            await _companyCatalogService.CreateNewCatalog(User.CompanyId(), createCatalogviewModel.SourceCatalogId, new CatalogModel
            {
                Name = createCatalogviewModel.Name,
                Description = createCatalogviewModel.Description,
                CompaniesAssignedToCatalog = createCatalogviewModel.CompaniesAssignedToCatalog.Select(companyId => new CatalogCompanyModel
                {
                    CompanyId = companyId,
                }).ToList()
            });
            return Ok(await _catalogHelper.GetCatalogs(User.CompanyId()));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("{catalogId:int}/reseller", Name = "GetCatalog")]
        public async Task<IHttpActionResult> Get(int catalogId)
        {
            return Ok(await _catalogHelper.GetCatalog(User.CompanyId(), catalogId));
        }

        [HttpPut]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog, PermissionsConstants.SetMsrpFixed)]
        [Route("{catalogId:int}/reseller", Name = "UpdateCatalog")]
        public async Task<IHttpActionResult> Update(int catalogId, UpdateCatalogViewModel model)
        {
            var catalog = await _companyCatalogService.GetCompanyCatalog(User.CompanyId(), catalogId);
            var myCompaniesAssignedToCatalog =
                await _companyCatalogService.GetCompaniesAssignedToCatalog(User.CompanyId(), catalogId);

            var newlyAssigned = model.CompaniesAssignedToCatalog.Where(o => model.CompaniesAssignedToCatalog.Any() &&
                                myCompaniesAssignedToCatalog.All(n => o != n.CompanyId)).Select(c => c);

            var removed = myCompaniesAssignedToCatalog.Where(n =>
                model.CompaniesAssignedToCatalog.All(o => n.CompanyId != o)).Select(c => c.CompanyId);

            foreach (var newlyAssignedCompanyId in newlyAssigned)
            {
                await _companyCatalogService.AssignCatalogToCompany(User.CompanyId(), newlyAssignedCompanyId, catalogId);
            }

            foreach (var removedCompanyId in removed)
            {
                await _companyCatalogService.RemoveCompanyFromAssignedCatalog(User.CompanyId(), removedCompanyId);
            }

            await _companyCatalogService.UpdateCompanyCatalog(User.CompanyId(), catalogId, new CatalogModel
            {
                Name = model.Name,
                Description = model.Description
            });

            foreach (var newModel in model.ProductItems)
            {
                //var currentModel = catalog.Products
                //                    .Select(p => p.ProductItems.FirstOrDefault(pi => pi.ProductItemId == newModel.ProductItemId))
                //                    .FirstOrDefault();
                var currentModel = catalog.Products.SelectMany(p => p.ProductItems).FirstOrDefault(i => i.ProductItemId == newModel.ProductItemId);
                if (currentModel == null)
                    throw new Exception();

                if (currentModel.Available != newModel.Available)
                {
                    await _catalogProductItemService.ChangeProductAvailability(User.CompanyId(), catalogId, newModel.ProductItemId, newModel.Available);
                }

                await _catalogHelper.UpdateFixedRetailPrice(User.UserId(), User.CompanyId(), currentModel, newModel, catalogId);

                await _catalogHelper.UpdateRetailPrice(User.UserId(), User.CompanyId(), currentModel, newModel, catalogId);

                await _catalogHelper.UpdateResellerPrice(User.CompanyId(), currentModel, newModel, catalogId);

            }

            return Ok(await _catalogHelper.GetCatalog(User.CompanyId(), catalogId));
        }
        [HttpDelete]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("{catalogId:int}/reseller", Name = "DeleteCatalog")]
        public async Task<IHttpActionResult> Delete(int catalogId)
        {
            await _companyCatalogService.DeleteCatalog(User.CompanyId(), catalogId);

            return Ok(await _catalogHelper.GetCatalogs(User.CompanyId()));
        }


        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("reseller/companies", Name = "GetCompaniesCatalogs")]
        public async Task<IHttpActionResult> GetCompaniesCatalogs()
        {
            var myCompaniesAssignedCatalogs = await _companyCatalogService.GetMyCompaniesAssignedCatalogs(User.CompanyId());
            return Ok(myCompaniesAssignedCatalogs.Select(c => c.ToCatalogCompanyViewModel()));
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog, PermissionsConstants.ViewPriceCatalog)]
        [Route("customer", Name = "GetCustomerCatalog")]
        public async Task<IHttpActionResult> GetCustomerCatalog()
        {
            var imageServerPath = $"{Url.Content("~/")}Static/Images/Office365Icons";

            var customerProducts = await _customerCatalogService.GetCustomerProducts(User.CompanyId());
            var customerProductsViewModel = customerProducts.Select(cp => cp.ToCustomerCatalogProductViewModel(imageServerPath));

            return Ok(customerProductsViewModel);
        }

        #region Send email with product details as attachment.
        //Implemented By TAG. 
        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("SendCatalogEmail", Name = "SendEmailCatalog")]
        public async Task<IHttpActionResult> SendEmailCatalog(EmailCatalogModel model)
        {
            var Email = false;
            try
            {
                var sendEmailQueue = NotificationServiceConstants.QueueSendEmailNotification;
                await _messageBroker.GetSendEndpoint(sendEmailQueue).Send<ISendEmailCommand>(
                  new
                  {
                      CompanyId = model.companyId,
                      To = model.recipients,
                      Body = model.body,
                      Subject = model.subject,
                      CatalogId = model.catalogId,
                      Attachment = true
                  });

                Email = true;
            }
            catch (Exception ex) { Email = false; }
            return Ok(Email);
        }
        #endregion

        #region download the product details.
        //Implemented By TAG. 
        // It get download the product details. And it will return the excel file.
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewPriceCatalog)]
        [Route("DownloadProductasExcel", Name = "DownloadProductasExcel")]
        public async Task<HttpResponseMessage> DownloadProductasExcel(int catalogId, int companyId)
        {
            var Product = await _catalogHelper.GetProductDetailsAsMemoryStream(companyId, catalogId);

            //_workbook.SaveAs(memoryStream);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Product.Item1.ToArray())
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue
                   ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition =
                                   new ContentDispositionHeaderValue("attachment")
                                   {
                                       FileName = $"{Regex.Replace(Product.Item2, @"\s+", "")}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"
                                   };
            return response;
        }
        #endregion

    }
}
