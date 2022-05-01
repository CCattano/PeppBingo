using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.Game;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.Facades
{
    /// <summary>
    /// Facade for working with the information we store that is
    /// explicitly related to the bingo boards we play with
    /// </summary>
    public interface IGameFacade
    {
        /// <summary>
        /// Inserts board data into the Boards table
        /// </summary>
        /// <param name="newBoard"></param>
        /// <returns></returns>
        Task<BoardBE> CreateBoard(BoardBE newBoard);
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
        ///Updates an existing Board w/ new information from the param provided
        /// </summary>
        /// <param name="boardBE"></param>
        /// <returns></returns>
        Task<BoardBE> UpdateBoard(BoardBE boardBE);
        /// <summary>
        /// Deletes an existing board, completely removing it from the Db
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task DeleteBoard(int boardID);
        /// <summary>
        /// Inserts board tile data into the Boards table
        /// </summary>
        /// <param name="newTile"></param>
        /// <returns></returns>
        Task<BoardTileBE> CreateBoardTile(BoardTileBE newTile);
        /// <summary>
        /// Fetches all board tiles for a given board
        /// </summary>
        /// <returns></returns>
        Task<List<BoardTileBE>> GetAllBoardTiles(int boardID);
        /// <summary>
        /// Updates an existing Board Tile w/ new information from the param provided
        /// </summary>
        /// <param name="boardTileBE"></param>
        /// <returns></returns>
        Task<BoardTileBE> UpdateBoardTile(BoardTileBE boardTileBE);
        /// <summary>
        /// Deletes an existing board tile, completely removing it from the Db
        /// </summary>
        /// <param name="tileID"></param>
        /// <returns></returns>
        Task DeleteBoardTile(int tileID);
        /// <summary>
        /// Deletes an existing board tiles for a specified board,
        /// completely removing them from the Db
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task DeleteAllBoardTilesForBoard(int boardID);
    }

    /// <inheritdoc cref="IGameFacade"/>
    public class GameFacade : IGameFacade
    {
        private readonly IMapper _mapper;
        private readonly IBingoDataService _dataSvc;

        public GameFacade(IMapper mapper, IBingoDataService dataSvc)
        {
            _mapper = mapper;
            _dataSvc = dataSvc;
        }

        public async Task<BoardBE> CreateBoard(BoardBE newBoard)
        {
            BoardEntity boardEntity = _mapper.Map<BoardEntity>(newBoard);
            await _dataSvc.Game.BoardRepo.InsertBoard(boardEntity);
            BoardBE boardBE = _mapper.Map<BoardBE>(boardEntity);
            return boardBE;
        }

        public async Task<BoardBE> GetBoard(int boardID)
        {
            BoardEntity boardEntity =
                await _dataSvc.Game.BoardRepo.GetBoard(boardID);
            BoardBE boardBE = _mapper.Map<BoardBE>(boardEntity);
            return boardBE;
        }

        public async Task<List<BoardBE>> GetAllBoards()
        {
            List<BoardEntity> boardEntities =
                await _dataSvc.Game.BoardRepo.GetAllBoards();
            List<BoardBE> boardBEs =
                boardEntities?.Select(board => _mapper.Map<BoardBE>(board)).ToList();
            return boardBEs;
        }

        public async Task<BoardBE> UpdateBoard(BoardBE boardBE)
        {
            BoardEntity boardEntity = await _dataSvc.Game.BoardRepo.GetBoard(boardBE.BoardID);
            if (boardEntity == null)
                throw new WebException(HttpStatusCode.BadRequest, "Could not update board");
            boardEntity = _mapper.Map(boardBE, boardEntity);
            await _dataSvc.Game.BoardRepo.UpdateBoard(boardEntity);
            BoardBE updatedBoard = _mapper.Map<BoardBE>(boardEntity);
            return updatedBoard;
        }

        public async Task DeleteBoard(int boardID)
        {
            await _dataSvc.Game.BoardRepo.DeleteBoard(boardID);
        }

        public async Task<BoardTileBE> CreateBoardTile(BoardTileBE newTile)
        {
            BoardTileEntity tileEntity = _mapper.Map<BoardTileEntity>(newTile);
            await _dataSvc.Game.BoardTileRepo.InsertBoardTile(tileEntity);
            BoardTileBE tileBE = _mapper.Map<BoardTileBE>(tileEntity);
            return tileBE;
        }

        public async Task<List<BoardTileBE>> GetAllBoardTiles(int boardID)
        {
            List<BoardTileEntity> boardTileEntities =
                await _dataSvc.Game.BoardTileRepo.GetBoardTiles(boardID);
            List<BoardTileBE> boardTileBEs =
                boardTileEntities?.Select(tile => _mapper.Map<BoardTileBE>(tile)).ToList();
            return boardTileBEs;
        }

        public async Task<BoardTileBE> UpdateBoardTile(BoardTileBE boardTileBE)
        {
            BoardTileEntity boardTileEntity = await _dataSvc.Game.BoardTileRepo.GetBoardTile(boardTileBE.TileID);
            /*
             * The boardID is not a property directly exposed on the front end
             * We have it thanks to the DB
             * And we should persist it on BEs in updates
             * So we're setting it on BE here so when the translator runs
             * The BE sets the boardID we gave it on the entity that goes to the Db
             */
            boardTileBE.BoardID = boardTileEntity.BoardID;
            if (boardTileEntity == null)
                throw new WebException(HttpStatusCode.BadRequest, "Could not update board tile");
            boardTileEntity = _mapper.Map(boardTileBE, boardTileEntity);
            await _dataSvc.Game.BoardTileRepo.UpdateBoardTile(boardTileEntity);
            BoardTileBE updatedBoard = _mapper.Map<BoardTileBE>(boardTileEntity);
            return updatedBoard;
        }

        public async Task DeleteBoardTile(int tileID)
        {
            await _dataSvc.Game.BoardTileRepo.DeleteBoardTile(tileID);
        }

        public async Task DeleteAllBoardTilesForBoard(int boardID)
        {
            await _dataSvc.Game.BoardTileRepo.DeleteAllBoardTilesForBoard(boardID);
        }
    }
}
