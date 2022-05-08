using Pepp.Web.Apps.Bingo.Data.Repos.Stats;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the Stats schema
    /// </summary>
    public interface IStatsSchema
    {
        /// <see cref="ILeaderboardRepo"/>
        ILeaderboardRepo LeaderboardRepo { get; }
    }

    /// <see cref="IStatsSchema"/>
    public class StatsSchema: BaseSchema, IStatsSchema
    {
        private ILeaderboardRepo _leaderboardRepo;

        public StatsSchema(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public ILeaderboardRepo LeaderboardRepo => _leaderboardRepo ??= new LeaderboardRepo(base.DataSvc);
    }
}