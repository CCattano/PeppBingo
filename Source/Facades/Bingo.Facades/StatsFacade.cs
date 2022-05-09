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
        /// Inserts leaderboard data into the Leaderboards table
        /// </summary>
        /// <param name="newLeaderboard"></param>
        /// <returns></returns>
        Task<LeaderboardBE> CreateLeaderboard(LeaderboardBE newLeaderboard);

        /// <summary>
        /// Get all Leaderboards we store data for
        /// </summary>
        /// <returns></returns>
        Task<List<LeaderboardBE>> GetAllLeaderboards();

        /// <summary>
        /// Get Leaderboard with <see cref="boardID"/> provided
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task<LeaderboardBE> GetLeaderboard(int boardID);

        /// <summary>
        /// Delete a leaderboard
        /// </summary>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task DeleteLeaderboard(int leaderboardID);

        /// <summary>
        /// Inserts new Leaderboard position for a user into the table
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task CreateLeaderboardPosition(int userID, int leaderboardID);

        /// <summary>
        /// Get user's leaderboard position information
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task<LeaderboardPosBE> GetLeaderboardPosition(int userID, int leaderboardID);

        /// <summary>
        /// Update a user's leaderboard position information
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task UpdateLeaderboardPosition(int userID, int leaderboardID);

        /// <summary>
        /// Delete all leaderboard positions for a single leaderboard
        /// </summary>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task DeleteAllLeaderboardPositions(int leaderboardID);
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

        #region Leaderboard Methods

        public async Task<LeaderboardBE> CreateLeaderboard(LeaderboardBE newLeaderboard)
        {
            LeaderboardEntity entity = _mapper.Map<LeaderboardEntity>(newLeaderboard);
            await _dataSvc.Stats.LeaderboardRepo.InsertLeaderboard(entity);
            LeaderboardBE leaderboardBE = _mapper.Map<LeaderboardBE>(entity);
            return leaderboardBE;
        }

        public async Task<List<LeaderboardBE>> GetAllLeaderboards()
        {
            List<LeaderboardEntity> leaderBoardEntities =
                await _dataSvc.Stats.LeaderboardRepo.GetAllLeaderboards();
            List<LeaderboardBE> leaderboardBEs =
                leaderBoardEntities?.Select(entity => _mapper.Map<LeaderboardBE>(entity)).ToList();
            return leaderboardBEs;
        }

        public async Task<LeaderboardBE> GetLeaderboard(int boardID)
        {
            LeaderboardEntity leaderBoardEntity =
                await _dataSvc.Stats.LeaderboardRepo.GetLeaderboard(boardID);
            LeaderboardBE leaderboardBE = _mapper.Map<LeaderboardBE>(leaderBoardEntity);
            return leaderboardBE;
        }

        public async Task DeleteLeaderboard(int leaderboardID)
        {
            await _dataSvc.Stats.LeaderboardRepo.DeleteLeaderboard(leaderboardID);
        }

        #endregion

        #region Leaderboard Position Methods

        public async Task CreateLeaderboardPosition(int userID, int leaderboardID) =>
            await _dataSvc.Stats.LeaderboardPosRepo.CreateLeaderboardPosition(userID, leaderboardID);

        public async Task<LeaderboardPosBE> GetLeaderboardPosition(int userID, int leaderboardID)
        {
            LeaderboardPosEntity leaderboardPositionEntity =
                await _dataSvc.Stats.LeaderboardPosRepo.GetLeaderboardPosition(userID, leaderboardID);
            LeaderboardPosBE leaderboardPosBE =
                _mapper.Map<LeaderboardPosBE>(leaderboardPositionEntity);
            return leaderboardPosBE;
        }

        public async Task UpdateLeaderboardPosition(int userID, int leaderboardID) =>
            await _dataSvc.Stats.LeaderboardPosRepo.UpdateLeaderboardPosition(userID, leaderboardID);

        public async Task DeleteAllLeaderboardPositions(int leaderboardID) =>
            await _dataSvc.Stats.LeaderboardPosRepo.DeleteAllLeaderboardPositions(leaderboardID);

        #endregion
    }
}