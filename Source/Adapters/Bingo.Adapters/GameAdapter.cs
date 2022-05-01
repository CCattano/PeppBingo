using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.Facades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IGameFacade _facade;

        public GameAdapter(IGameFacade facade) => _facade = facade;

        public async Task<BoardBE> GetBoard(int boardID)
        {
            BoardBE boardBE = await _facade.GetBoard(boardID);
            return boardBE;
        }

        public async Task<List<BoardBE>> GetAllBoards()
        {
            List<BoardBE> boardBEs = await _facade.GetAllBoards();
            return boardBEs;
        }

        public async Task<BoardBE> CreateBoard(int userID, BoardBE newBoard)
        {
            newBoard.CreatedBy = newBoard.ModBy = userID;
            BoardBE boardBE = await _facade.CreateBoard(newBoard);
            return boardBE;
        }

        public async Task<BoardBE> UpdateBoard(int userID, BoardBE boardBE)
        {
            boardBE.ModBy = userID;
            BoardBE updatedBoard = await _facade.UpdateBoard(boardBE);
            return updatedBoard;
        }

        public async Task DeleteBoard(int boardID)
        {
            await _facade.DeleteAllBoardTilesForBoard(boardID);
            await _facade.DeleteBoard(boardID);
        }

        public async Task<List<BoardTileBE>> GetAllBoardTiles(int boardID)
        {
            List<BoardTileBE> boardTileBEs = await _facade.GetAllBoardTiles(boardID);
            return boardTileBEs;
        }

        public async Task<BoardTileBE> CreateBoardTile(int userID, int boardID, BoardTileBE newTile)
        {
            newTile.CreatedBy = newTile.ModBy = userID;
            newTile.BoardID = boardID;
            BoardTileBE boardTileBE = await _facade.CreateBoardTile(newTile);
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
            BoardTileBE updatedBoardTile = await _facade.UpdateBoardTile(boardTileBE);
            return updatedBoardTile;
        }

        public async Task DeleteBoardTile(int tileID)
        {
            await _facade.DeleteBoardTile(tileID);
        }
    }
}
