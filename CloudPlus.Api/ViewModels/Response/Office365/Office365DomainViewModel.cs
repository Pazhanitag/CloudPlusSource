using CloudPlus.Enums.Office365;
using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Office365
{
    public class Office365DomainViewModel
    {
        public Office365DomainViewModel()
        {
            Users = new List<Office365DomainUserViewModel>();
        }

        public string Name { get; set; }
        public Office365DomainStatus? Status { get; set; }
        public bool VerificationInProgress { get; set; }
        public bool ConfigurationInProgress { get; set; }
        public bool IsFederated { get; set; }
        public IEnumerable<Office365DomainUserViewModel> Users { get; set; }   
    }
}