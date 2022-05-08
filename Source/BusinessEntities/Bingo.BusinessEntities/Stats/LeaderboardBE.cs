namespace Pepp.Web.Apps.Bingo.BusinessEntities.Stats
{
    /// <summary>
    /// Metadata about a Leaderboard and the Bingo Board it represents
    /// </summary>
    public class LeaderboardBE
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