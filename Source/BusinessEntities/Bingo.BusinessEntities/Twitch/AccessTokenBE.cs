namespace Pepp.Web.Apps.Bingo.BusinessEntities.Twitch
{
    /// <summary>
    /// Access Token information extracted from the Twitch API
    /// </summary>
    public class AccessTokenBE
    {
        /// <summary>
        /// Twitch-provided unique UserID
        /// </summary>
        public string TwitchUserID { get; init; }
        /// <summary>
        /// Twitch provided Access Token
        /// </summary>
        public string AccessToken;
        /// <summary>
        /// Twitch provided Refresh Token
        /// </summary>
        public string RefreshToken;

        public AccessTokenBE()
        {
        }

        public AccessTokenBE(string twitchUserID) => TwitchUserID = twitchUserID;
    }
}
