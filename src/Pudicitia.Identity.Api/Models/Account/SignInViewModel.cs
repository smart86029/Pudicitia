namespace Pudicitia.Identity.Api.Models.Account
{
    public class SignInViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}