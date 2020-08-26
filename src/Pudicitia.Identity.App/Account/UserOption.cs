using System.ComponentModel.DataAnnotations;

namespace Pudicitia.Identity.App.Account
{
    public class SignInCommand
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}