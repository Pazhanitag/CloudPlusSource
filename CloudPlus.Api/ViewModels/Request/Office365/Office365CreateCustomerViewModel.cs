using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365CreateCustomerViewModel
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string Domain { get; set; }
        public string Culture => "en-US";
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Language => "en";
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string PostalCode { get; set; }
    }
}