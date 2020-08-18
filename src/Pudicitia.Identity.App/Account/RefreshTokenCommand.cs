using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Account
{
    public class RefreshTokenCommand : Command
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
