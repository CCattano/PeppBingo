namespace Pepp.Web.Apps.Bingo.Data.Entities.Twitch
{
    /// <summary>
    /// Access Token information provided via the Twitch API
    /// </summary>
    public class AccessTokenEntity
    {
        /// <summary>
        /// Twitch-provided unique UserID
        /// </summary>
        public string UserID;
        /// <summary>
        /// Twitch provided Access Token
        /// </summary>
        public string Token;
        /// <summary>
        /// Twitch provided Refresh Token
        /// </summary>
        public string RefreshToken;
    }
}
