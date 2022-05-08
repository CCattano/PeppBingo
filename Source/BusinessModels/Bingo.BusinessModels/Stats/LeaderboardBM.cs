namespace Pepp.Web.Apps.Bingo.BusinessModels.Stats
{
    /// <summary>
    /// Metadata about a Leaderboard and the Bingo Board it represents
    /// </summary>
    public class LeaderboardBM
    {
        /// <summary>
        /// Primary Key for the Leaderboard Table
        /// </summary>
        public int LeaderboardID { get; set; }

        /// <summary>
        /// The name of the bingo board this leaderboard is for
        /// </summary>
        public string BoardName { get; set; }
    }
}