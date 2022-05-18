using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Stats
{
    public class LeaderboardPosEntity_LeaderboardPosBE : ITypeConverter<LeaderboardPosEntity, LeaderboardPosBE>, ITypeConverter<LeaderboardPosBE, LeaderboardPosEntity>
    {
        public LeaderboardPosEntity Convert(LeaderboardPosBE source, LeaderboardPosEntity destination, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
        
        public LeaderboardPosBE Convert(LeaderboardPosEntity source, LeaderboardPosBE destination, ResolutionContext context)
        {
            LeaderboardPosBE result = destination ?? new();
            result.LeaderboardPosID = source.LeaderboardPosID;
            result.LeaderboardID = source.LeaderboardID;
            result.UserID = source.UserID;
            result.BingoQty = source.BingoQty;
            return result;
        }
    }
}