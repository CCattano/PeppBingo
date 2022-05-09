using System.Collections.Generic;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;
using Pepp.Web.Apps.Bingo.Facades;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    /// <summary>
    /// Adapter responsible for the business logic surrounding stat/metric keeping
    /// </summary>
    public interface IStatsAdapter
    {
        /// <summary>
        /// Get all Leaderboards
        /// </summary>
        /// <returns></returns>
        Task<List<LeaderboardBE>> GetAllLeaderboards();
        /// <summary>
        /// Update a user's leaderboard position if it exists or create
        /// a new leaderboard position for the user if it does not
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task UpdatePosition(int userID, int boardID);
    }

    public class StatsAdapter : IStatsAdapter
    {
        private readonly IStatsFacade _facade;

        public StatsAdapter(IStatsFacade facade) =>
            _facade = facade;

        public async Task<List<LeaderboardBE>> GetAllLeaderboards()
        {
            List<LeaderboardBE> leaderboardBEs = await _facade.GetAllLeaderboards();
            return leaderboardBEs;
        }

        public async Task UpdatePosition(int userID, int boardID)
        {
            LeaderboardBE leaderboardBE = await _facade.GetLeaderboard(boardID);
            LeaderboardPosBE userLeaderboardPosition =
                await _facade.GetLeaderboardPosition(userID, leaderboardBE.LeaderboardID);
            if (userLeaderboardPosition == null)
                await _facade.CreateLeaderboardPosition(userID, leaderboardBE.LeaderboardID);
            else
                await _facade.UpdateLeaderboardPosition(userID, leaderboardBE.LeaderboardID);
        }
    }
}