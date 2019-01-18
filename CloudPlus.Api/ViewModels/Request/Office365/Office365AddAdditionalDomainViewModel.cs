using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365AddAdditionalDomainViewModel
    {
        [Required]
        public string Office365CustomerId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string Domain { get; set; }
        [Required]
        public string Email { get; set; }
    }
}