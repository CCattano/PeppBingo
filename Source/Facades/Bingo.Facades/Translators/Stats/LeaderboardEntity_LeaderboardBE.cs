using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Stats
{
    public class LeaderboardEntity_LeaderboardBE: ITypeConverter<LeaderboardEntity, LeaderboardBE>, ITypeConverter<LeaderboardBE, LeaderboardEntity>
    {
        public LeaderboardEntity Convert(LeaderboardBE source, LeaderboardEntity destination, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
        
        public LeaderboardBE Convert(LeaderboardEntity source, LeaderboardBE destination, ResolutionContext context)
        {
            LeaderboardBE result = destination ?? new();
            result.LeaderboardID = source.LeaderboardID;
            result.BoardID = source.BoardID;
            return result;
        }
    }
}