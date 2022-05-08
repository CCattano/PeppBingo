namespace Pepp.Web.Apps.Bingo.BusinessModels.Stats
{
    /// <summary>
    /// Leaderboard Position for a single user for a single bingo board
    /// </summary>
    public class LeaderboardPosBM
    {
        /// <summary>
        /// The ID of the Leaderboard this position is assoc. w/
        /// </summary>
        public int LeaderboardID { get; set; }

        /// <summary>
        /// Twitch-provided display name for a User
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Uri to fetch a User's Twitch profile image
        /// </summary>
        public string ProfileImageUri { get; set; }

        /// <summary>
        /// The number of bingos this user has for this bingo board
        /// </summary>
        public int BingoQty { get; set; }
    }
}