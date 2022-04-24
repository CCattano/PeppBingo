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
    }
}
