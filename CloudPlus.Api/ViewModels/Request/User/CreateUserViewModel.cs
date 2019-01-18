using System.ComponentModel.DataAnnotations;
using CloudPlus.Enums.User;
using ExpressiveAnnotations.Attributes;

namespace CloudPlus.Api.ViewModels.Request.User
{
    public class CreateUserViewModel
    {
        public int CompanyId {  get; set; }
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Domain { get; set; }
        public string Email => $"{UserName}@{Domain}";
        public string DisplayName { get; set; }
        [EmailAddress]
        public string AlternativeEmail { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
	    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string State { get; set; }
        public string City { get; set; }
	    [StringLength(10)]
	    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public PasswordSetupMethod PasswordSetupMethod { get; set; }
        public bool SendPlainPasswordViaEmail { get; set; }

        [EmailAddress]
        [RequiredIf("PasswordSetupMethod == 1", ErrorMessage = "PasswordSetupEmail is required")]
        public string PasswordSetupEmail { get; set; }
        [EmailAddress]
        [RequiredIf("PasswordSetupMethod == 1", ErrorMessage = "PasswordSetupEmail is required")]
        [AssertThat("PasswordSetupEmailRetyped == PasswordSetupEmail", ErrorMessage = "PasswordSetupEmailRetyped needs to be the same as PasswordSetupEmail")]
        public string PasswordSetupEmailRetyped { get; set; }

        [RequiredIf("PasswordSetupMethod == 2", ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [RequiredIf("PasswordSetupMethod == 2", ErrorMessage = "PasswordRetyped is required")]
        [AssertThat("PasswordRetyped == Password", ErrorMessage = "PasswordRetyped needs to be the same as Password")]
        public string PasswordRetyped { get; set; }

        public UserStatus UserStatus { get; set; }
        public bool SendWelcomeLetters { get; set; }

        [Required]
        [MaxLength(1)]
        [MinLength(1)]
        public int[] Roles { get; set; }

        public string AvatarBase64 { get; set; }
        public string ProfilePicture { get; set; }
    }
}