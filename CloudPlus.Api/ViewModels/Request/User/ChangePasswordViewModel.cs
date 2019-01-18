using CloudPlus.Enums.User;
using ExpressiveAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.User
{
    public class ChangePasswordViewModel
    {
        [Required]
        public int UserId { get; set; }
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
    }
}