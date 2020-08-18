using System.Text.Json.Serialization;
using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Account
{
    public class CreateTokenCommand : Command
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
    }
}