using Pudicitia.Common.Commands;

namespace Pudicitia.Identity.App.Account
{
    public class RefreshTokenCommand : ICommand<TokenDetail>
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
