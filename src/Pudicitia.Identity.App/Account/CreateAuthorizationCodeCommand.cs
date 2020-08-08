namespace Pudicitia.Identity.App.Account
{
    public class CreateAuthorizationCodeCommand
    {
        public string ResponseType { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string Scope { get; set; }

        public string State { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}