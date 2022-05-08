using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;

namespace Pepp.Web.Apps.Bingo.Facades
{
    /// <summary>
    /// Facade for working with the information we store that is
    /// related to our stat and metric keeping
    /// </summary>
    public interface IStatsFacade
    {
        /// <summary>
        /// Get all Leaderboards we store data for
        /// </summary>
        /// <returns></returns>
        Task<List<LeaderboardBE>> GetAllLeaderboards();
    }

    public class StatsFacade : IStatsFacade
    {
        private readonly IMapper _mapper;
        private readonly IBingoDataService _dataSvc;

        public StatsFacade(IMapper mapper, IBingoDataService dataSvc)
        {
            _mapper = mapper;
            _dataSvc = dataSvc;
        }

        public async Task<List<LeaderboardBE>> GetAllLeaderboards()
        {
            List<LeaderboardEntity> leaderBoardEntities =
                await _dataSvc.Stats.LeaderboardRepo.GetAllLeaderboards();
            List<LeaderboardBE> leaderboardBEs =
                leaderBoardEntities?.Select(entity => _mapper.Map<LeaderboardBE>(entity)).ToList();
            return leaderboardBEs;
        }
    }
}