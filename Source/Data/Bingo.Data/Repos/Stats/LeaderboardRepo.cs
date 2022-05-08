using System.Collections.Generic;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;

namespace Pepp.Web.Apps.Bingo.Data.Repos.Stats
{
    /// <summary>
    /// Repo used to interface with data stored in the game.Board table
    /// </summary>
    public interface ILeaderboardRepo
    {
        /// <summary>
        /// Fetches all Leaderboard information stored in the table
        /// </summary>
        /// <returns></returns>
        Task<List<LeaderboardEntity>> GetAllLeaderboards();
    }

    /// <inheritdoc cref="ILeaderboardRepo"/>
    public class LeaderboardRepo : BaseRepo, ILeaderboardRepo
    {
        public LeaderboardRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task<List<LeaderboardEntity>> GetAllLeaderboards()
        {
            List<LeaderboardEntity> queryData =
                await base.Read<LeaderboardEntity>(Sprocs.GetAllLeaderboards);
            return queryData;
        }
        
        private struct Sprocs
        {
            public const string GetAllLeaderboards = "stats.usp_SELECT_AllLeaderboards";
        }
    }
}