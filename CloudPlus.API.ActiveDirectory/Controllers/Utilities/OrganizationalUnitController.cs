using System.Web.Http;
using CloudPlus.Api.ActiveDirectory.Database;

namespace CloudPlus.Api.ActiveDirectory.Controllers.Utilities
{
    public class OrganizationalUnitController : ApiController
    {
        private readonly IOrganizationalUnitRepository _organizationalUnitRepository;

        public OrganizationalUnitController(IOrganizationalUnitRepository organizationalUnitRepository)
        {
            _organizationalUnitRepository = organizationalUnitRepository;
        }
        
        [HttpGet]
        public IHttpActionResult GetNextOuId()
        {
            return Ok(_organizationalUnitRepository.GenerateNewOuId());
        }
    }
}