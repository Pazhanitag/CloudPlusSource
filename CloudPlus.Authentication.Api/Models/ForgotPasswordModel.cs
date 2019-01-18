using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Authentication.Api.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string ClientRedirectEndpoint { get; set; }
        public string SignInMessage { get; set; }
        public string Token { get; set; }
    }
}