using System.Text.Json.Serialization;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models
{
    public class TwitchAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; }
    }
}
