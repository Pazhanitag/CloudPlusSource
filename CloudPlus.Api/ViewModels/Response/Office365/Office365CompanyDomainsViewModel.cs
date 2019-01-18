using CloudPlus.Models.Office365.Api;
using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Office365
{
    public class Office365CompanyDomainsViewModel
    {
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public IEnumerable<Office365DomainViewModel> Domains { get; set; }
        public string Office365CustomerId { get; set; }
    }
}