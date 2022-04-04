namespace Pepp.Web.Apps.Bingo.Data.Entities.User
{
    /// <summary>
    /// Information about a User
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// Internal User ID to represent a User
        /// </summary>
        public int UserID;
        /// <summary>
        /// Twitch-provided unique UserID
        /// </summary>
        public string TwitchUserID;
        /// <summary>
        /// Twitch-provided display name for a User
        /// </summary>
        public string DisplayName;
        /// <summary>
        /// Uri to fetch a User's Twitch profile image
        /// </summary>
        public string ProfileImageUri;
    }
}
