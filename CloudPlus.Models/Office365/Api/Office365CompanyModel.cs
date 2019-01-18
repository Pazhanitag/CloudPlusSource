using System.Collections.Generic;
using CloudPlus.Models.Office365.Domain;

namespace CloudPlus.Models.Office365.Api
{
    public class Office365CompanyModel
    {
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public IEnumerable<Office365DomainModel> Domains { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
