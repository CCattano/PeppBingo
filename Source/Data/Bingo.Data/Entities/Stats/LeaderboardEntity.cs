namespace Pepp.Web.Apps.Bingo.Data.Entities.Stats
{
    /// <summary>
    /// Metadata about a Leaderboard and the Bingo Board it represents
    /// </summary>
    public class LeaderboardEntity
    {
        /// <summary>
        /// Primary Key for the Leaderboard Table
        /// </summary>
        public int LeaderboardID;

        /// <summary>
        /// The ID of the Bingo Board this leaderboard represents
        /// </summary>
        public int BoardID;
    }
}