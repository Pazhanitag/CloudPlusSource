using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using CloudPlus.Api.Attributes;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Identity.User;
using CloudPlus.Api.Extensions;
using CloudPlus.Api.Mappers;
using CloudPlus.Constants;

namespace CloudPlus.Api.Controllers.Company
{
    [Authorize]
    [RoutePrefix("api/mycompany")]
    public class MyCompanyController : ApiController
    {
        private readonly IDomainService _domainService;
        private readonly IUserService _userService;

        public MyCompanyController(
            IDomainService domainService,
            IUserService userService)
        {
            _domainService = domainService;
            _userService = userService;
        }

        [HttpGet]
        [AuthorizeAccess(PermissionsConstants.EditMyCompany)]
		[Route("users", Name = "GetDomainUsers")]
        public IHttpActionResult GetDomainUsers(string domainName)
        {
            var domain = _domainService.GetDomainByName(domainName);
            if (domain.CompanyId != User.CompanyId())
            {
                return NotFound();
            }

            var response = _userService.GetUsersByDomain(domainName, User.CompanyId()).Select(u => u.ToOffice365DomainUserViewModel(Url.Content("~/")));

            return Ok(response);
        }
    }
}