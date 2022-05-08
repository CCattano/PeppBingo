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
    }
}