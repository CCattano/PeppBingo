namespace Pepp.Web.Apps.Bingo.Data.Entities.Stats
{
    /// <summary>
    /// Leaderboard Position for a single user for a single bingo board
    /// </summary>
    public class LeaderboardPosEntity
    {
        /// <summary>
        /// Primary Key for the Leaderboard Pos Table
        /// </summary>
        public int LeaderboardPosID;

        /// <summary>
        /// The ID of the Leaderboard this position is assoc. w/
        /// </summary>
        public int LeaderboardID;

        /// <summary>
        /// The user this leaderboard position represents
        /// </summary>
        public int UserID;

        /// <summary>
        /// The number of bingos this user has for this bingo board
        /// </summary>
        public int BingoQty;
    }
}