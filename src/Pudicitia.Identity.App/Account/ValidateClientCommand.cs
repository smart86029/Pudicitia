using System.Text.Json.Serialization;

namespace Pudicitia.Identity.App.Account
{
    public class ValidateClientCommand
    {
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}