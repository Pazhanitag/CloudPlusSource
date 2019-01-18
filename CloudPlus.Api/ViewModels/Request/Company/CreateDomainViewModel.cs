using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Company
{
    public class CreateDomainViewModel
    {
        [Required]
        public string Name { get; set; }
        public bool IsPrimary { get; set; }
    }
}