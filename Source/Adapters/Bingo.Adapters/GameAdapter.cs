using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.Facades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    /// <summary>
    /// Adapter responsible for the business logic surrounding board data
    /// </summary>
    public interface IGameAdapter
    {
        /// <summary>
        /// Create a new Bingo board that can contain tiles
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="newBoard"></param>
        /// <returns></returns>
        Task<BoardBE> CreateBoard(int userID, BoardBE newBoard);
        /// <summary>
        /// Fetches a board with the <paramref name="boardID"/> provided
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task<BoardBE> GetBoard(int boardID);
        /// <summary>
        /// Fetches all board maintained by admins in the application
        /// </summary>
        /// <returns></returns>
        Task<List<BoardBE>> GetAllBoards();
        /// <summary>
        /// Fetches all boards maintained by admins in the application
        /// with a BoardID found in the <paramref name="boardIDs"/> list
        /// </summary>
        /// <param name="boardIDs"></param>
        /// <returns></returns>
        Task<List<BoardBE>> GetBoards(List<int> boardIDs);
        /// <summary>
        /// Update an existing board w/ new information
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="boardBE"></param>
        /// <returns></returns>
        Task<BoardBE> UpdateBoard(int userID, BoardBE boardBE);
        /// <summary>
        /// Delete a board and all the tiles associated with it
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task DeleteBoard(int boardID);
        /// <summary>
        /// Create a new board tile for a bingo board
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="boardID"></param>
        /// <param name="newTile"></param>
        /// <returns></returns>
        Task<BoardTileBE> CreateBoardTile(int userID, int boardID, BoardTileBE newTile);
        /// <summary>
        /// Fetches all board tiles for a given board
        /// </summary>
        /// <returns></returns>
        Task<List<BoardTileBE>> GetAllBoardTiles(int boardID);
        /// <summary>
        /// Fetches all enabled board tiles for a given board
        /// </summary>
        /// <returns></returns>
        Task<List<BoardTileBE>> GetAllActiveBoardTiles(int boardID);
        /// <summary>
        /// Update an existing board tile w/ new information
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="boardTileBE"></param>
        /// <returns></returns>
        Task<BoardTileBE> UpdateBoardTile(int userID, BoardTileBE boardTileBE);
        /// <summary>
        /// Delete a board tile
        /// </summary>
        /// <param name="tileID"></param>
        /// <returns></returns>
        Task DeleteBoardTile(int tileID);
    }

    /// <inheritdoc cref="IGameAdapter"/>
    public class GameAdapter : IGameAdapter
    {
        private readonly IGameFacade _gameFacade;
        private readonly IStatsFacade _statsFacade;

        public GameAdapter(IGameFacade gameFacade, IStatsFacade statsFacade)
        {
            _gameFacade = gameFacade;
            _statsFacade = statsFacade;
        }

        public async Task<BoardBE> GetBoard(int boardID)
        {
            BoardBE boardBE = await _gameFacade.GetBoard(boardID);
            return boardBE;
        }

        public async Task<List<BoardBE>> GetAllBoards()
        {
            List<BoardBE> boardBEs = await _gameFacade.GetAllBoards();
            return boardBEs;
        }

        public async Task<List<BoardBE>> GetBoards(List<int> boardIDs)
        {
            List<BoardBE> boardBEs = await _gameFacade.GetBoards(boardIDs);
            return boardBEs;
        }

        public async Task<BoardBE> CreateBoard(int userID, BoardBE newBoard)
        {
            newBoard.CreatedBy = newBoard.ModBy = userID;
            BoardBE boardBE = await _gameFacade.CreateBoard(newBoard);
            LeaderboardBE newLeaderboard = new()
            {
                BoardID = boardBE.BoardID
            };
            await _statsFacade.CreateLeaderboard(newLeaderboard);
            return boardBE;
        }

        public async Task<BoardBE> UpdateBoard(int userID, BoardBE boardBE)
        {
            boardBE.ModBy = userID;
            BoardBE updatedBoard = await _gameFacade.UpdateBoard(boardBE);
            return updatedBoard;
        }

        public async Task DeleteBoard(int boardID)
        {
            await _gameFacade.DeleteAllBoardTilesForBoard(boardID);
            await _gameFacade.DeleteBoard(boardID);
            LeaderboardBE leaderboard = await _statsFacade.GetLeaderboard(boardID);
            await _statsFacade.DeleteAllLeaderboardPositions(leaderboard.LeaderboardID);
            await _statsFacade.DeleteLeaderboard(leaderboard.LeaderboardID);
        }

        public async Task<List<BoardTileBE>> GetAllBoardTiles(int boardID)
        {
            List<BoardTileBE> boardTileBEs = await _gameFacade.GetAllBoardTiles(boardID);
            return boardTileBEs;
        }

        public async Task<BoardTileBE> CreateBoardTile(int userID, int boardID, BoardTileBE newTile)
        {
            newTile.CreatedBy = newTile.ModBy = userID;
            newTile.BoardID = boardID;
            BoardTileBE boardTileBE = await _gameFacade.CreateBoardTile(newTile);
            return boardTileBE;
        }

        public async Task<List<BoardTileBE>> GetAllActiveBoardTiles(int boardID)
        {
            List<BoardTileBE> boardTileBEs = await GetAllBoardTiles(boardID);
            boardTileBEs = boardTileBEs?.Where(tile => tile.IsActive).ToList();
            return boardTileBEs;
        }

        public async Task<BoardTileBE> UpdateBoardTile(int userID, BoardTileBE boardTileBE)
        {
            boardTileBE.ModBy = userID;
            BoardTileBE updatedBoardTile = await _gameFacade.UpdateBoardTile(boardTileBE);
            return updatedBoardTile;
        }

        public async Task DeleteBoardTile(int tileID)
        {
            await _gameFacade.DeleteBoardTile(tileID);
        }
    }
}
