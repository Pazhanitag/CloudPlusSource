using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudPlus.Api.ViewModels.Request.User;

namespace CloudPlus.Api.ViewModels.Request.Company
{
    public class UpdateCompanyViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Website { get; set; }
        public string LogoBase64 { get; set; }
        public string Logo { get; set; }
        public string SupportSiteUrl { get; set; }
        public string ControlPanelSiteUrl { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string ZipCode { get; set; }
	    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string State { get; set; }
        [Required]
        public string Country { get; set; }
        public string BrandColorPrimary { get; set; }
        public string BrandColorSecondary { get; set; }
        public string BrandColorText { get; set; }
        public int? CatalogId { get; set; }
        public List<CreateDomainViewModel> Domains { get; set; }
    }
}