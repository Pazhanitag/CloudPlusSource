using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.User
{
    public class UpdatePasswordViewModel
    {

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
    }
}