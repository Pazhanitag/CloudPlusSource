using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ActiveDirectory.Models.Company
{
    public class CreateCompany : IActiveDirectoryModel
    {
        [Required]
        public int CompanyOu { get; set; }
    }
}
