using System.Text.Json.Serialization;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models
{
    /// <summary>
    /// User information retained and provided by Twitch via the Twitch API
    /// </summary>
    public class TwitchUser
    {
        /// <summary>
        /// User’s ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string UserID { get; init; }
        /// <summary>
        /// User’s login name.
        /// </summary>
        [JsonPropertyName("login")]
        public string LoginName { get; init; }
        /// <summary>
        /// User’s display name.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; init; }
        /// <summary>
        /// User’s type
        /// </summary>
        /// <remarks>
        /// Values can be "staff", "admin", "global_mod", or "".
        /// </remarks>
        [JsonPropertyName("type")]
        public string Type { get; init; }
        /// <summary>
        /// User’s broadcaster type
        /// </summary>
        /// <remarks>
        /// Values can be "partner", "affiliate", or "".
        /// </remarks>
        [JsonPropertyName("broadcaster_type")]
        public string BroadcasterTye { get; init; }
        /// <summary>
        /// User’s channel description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; init; }
        /// <summary>
        /// URL of the user’s profile image.
        /// </summary>
        [JsonPropertyName("profile_image_url")]
        public string ProfileImageUri { get; init; }
        /// <summary>
        /// URL of the user’s offline image.
        /// </summary>
        [JsonPropertyName("offline_image_url")]
        public string OfflineImageUri { get; init; }
        /// <summary>
        /// Total number of views of the user’s channel.
        /// </summary>
        [JsonPropertyName("view_count")]
        public int ViewCount { get; init; }
        /// <summary>
        /// User’s verified email address.
        /// </summary>
        /// <remarks>
        /// Returned if the request includes the user:read:email scope.
        /// </remarks>
        [JsonPropertyName("email")]
        public string Email { get; init; }
        /// <summary>
        /// Date when the user was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; init; }
    }
}
