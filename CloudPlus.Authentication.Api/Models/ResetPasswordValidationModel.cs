using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Authentication.Api.Models
{
    public class ResetPasswordValidationModel
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string Token { get; set; }
    }
}