using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models
{
    public class TwitchAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("scope")]
        public List<string> Scope { get; init; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; }
    }
}
