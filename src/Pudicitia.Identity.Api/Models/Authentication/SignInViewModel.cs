using System.ComponentModel.DataAnnotations;

namespace Pudicitia.Identity.Api.Models.Authentication
{
    public class SignInViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}