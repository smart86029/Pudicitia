﻿using System.Text.Json.Serialization;
using Pudicitia.Common.Commands;

namespace Pudicitia.Identity.App.Account
{
    public class CreateTokenCommand : ICommand<TokenDetail>
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