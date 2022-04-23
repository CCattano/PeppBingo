using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.Facades;
using System.Collections.Generic;
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
        /// Fetches all board maintained by admins in the application
        /// </summary>
        /// <returns></returns>
        Task<List<BoardBE>> GetAllBoards();
    }

    /// <inheritdoc cref="IGameAdapter"/>
    public class GameAdapter : IGameAdapter
    {
        private readonly IGameFacade _facade;

        public GameAdapter(IGameFacade facade) => _facade = facade;

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
    }
}
