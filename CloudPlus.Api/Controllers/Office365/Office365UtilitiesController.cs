using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Models.Office365.Utilities;
using CloudPlus.QueueModels.Office365.AddressValidation;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.QueueModels.Office365.Domain.Federate;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Utilities;

namespace CloudPlus.Api.Controllers.Office365
{
    [Authorize]
    [RoutePrefix("api/rpc/office365utilities")]
    public class Office365UtilitiesController : ApiController
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IOffice365DbUtilitiesService _office365UtilitiesService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;

        public Office365UtilitiesController(IMessageBroker messageBroker, IOffice365DbUtilitiesService office365UtilitiesService, IOffice365DbCustomerService office365DbCustomerService)
        {
            _messageBroker = messageBroker;
            _office365UtilitiesService = office365UtilitiesService;
            _office365DbCustomerService = office365DbCustomerService;
        }

        [Route("validatecustomeraddress")]
        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		public async Task<IHttpActionResult> ValidateCustomerAddress(Office365CustomerAddressViewModel address)
        {
            var checkAddressEndpoint = Office365ServiceConstants.Office365AddressValidationUri;

            var client =
                _messageBroker.GetRequestClient<IOffice365AddresValidationRequest, IOffice365AddresValidationResponse>(
                    checkAddressEndpoint, TimeSpan.FromSeconds(60));

            var response = await client.Request(address.ToOffice365AddresValidationRequest());

            return Ok(response);
        }

        [Route("verifycustomerdomain")]
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		public async Task<IHttpActionResult> VerifyCustomerDomain(string domainName)
        {
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(User.CompanyId());

            if (office365Customer == null)
                throw new NullReferenceException(nameof(office365Customer));

            var verifyDomainUri = Office365ServiceConstants.QueueOffice365DomainVerification;

            await _messageBroker.GetSendEndpoint(verifyDomainUri)
                .Send<IOffice365VerifyDomainCommand>(new
                {
                    DomainName = domainName,
                    office365Customer.Office365CustomerId
                });

            return Ok();
        }

        [Route("federatecustomerdomain")]
        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public async Task<IHttpActionResult> FederateCustomerDomain(string domainName)
        {
            var checkAddressEndpoint = Office365ServiceConstants.Office365AddressValidationUri;

            var client =
                _messageBroker.GetRequestClient<IOffice365FederateDomainRequest, IOffice365FederateDomainResponse>(
                    checkAddressEndpoint, TimeSpan.FromMinutes(3));

            var response = await client.Request(new Office365FederateDomainRequest
            {
                CompanyId = User.CompanyId(),
                DomainName = domainName
            });

            return Ok(response);
        }

        [Route("resendtxtrecords")]
        [HttpPost]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		public async Task<IHttpActionResult> ResendTxtRecords([FromBody]ResendTxtRecordsViewModel model)
        {
            if (User.CompanyId() != model.CompanyId)
                return NotFound();

            var queueOffice365ResendTxtRecords = Office365ServiceConstants.QueueOffice365ResendTxtRecords;

            await _messageBroker.GetSendEndpoint(queueOffice365ResendTxtRecords)
                .Send<IOffice365ResendTxtRecordsCommand>(new
                {
                    model.CompanyId,
                    model.Domain,
                    model.Email
                });

            return Ok();
        }

        [Route("getprovisioningstatus/{companyId:int}")]
        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
		public IHttpActionResult GetProvisioningStatus(int companyId)
        {
            var provisioningStatus = _office365UtilitiesService.CheckProvisioningStatus(companyId);

            return Ok(provisioningStatus);
        }

        #region Get available services for Compatible matrix for office 365
        [Route("GetOffice365Services")]
        [HttpGet]
        //[AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public IHttpActionResult GetOffice365Services() {
            var ProductItemsOffice365 = _office365UtilitiesService.GetOffice365CompatibleMatrix();
            return Ok(ProductItemsOffice365);
        }
        #endregion

        #region Insert Office 365 Compatible matrix by bulk
        [Route("AddUpdateCompatibleMatrix")]
        [HttpPost]
        //[AuthorizeAccess(PermissionsConstants.ViewProductCatalog)]
        public IHttpActionResult AddUpdateCompatibleMatrix(List<Office365CompatabileMatrix> lstOffice365CompatabileMatrix)
        {
            var AddUpdateStatus = _office365UtilitiesService.AddUpdateCompatibleMatrix(lstOffice365CompatabileMatrix);
            return Ok(AddUpdateStatus);
        }
        #endregion
    }
}
