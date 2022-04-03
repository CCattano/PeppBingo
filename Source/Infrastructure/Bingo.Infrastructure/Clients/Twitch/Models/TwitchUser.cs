﻿using System.Text.Json.Serialization;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models
{
    public class TwitchUser
    {
        [JsonPropertyName("id")]
        public string UserID { get; set; }
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
        [JsonPropertyName("profile_image_url")]
        public string ProfileImageUri { get; set; }
    }
}
