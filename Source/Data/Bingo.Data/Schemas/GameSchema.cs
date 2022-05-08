using Pepp.Web.Apps.Bingo.Data.Repos.Game;

namespace Pepp.Web.Apps.Bingo.Data.Schemas
{
    /// <summary>
    /// Interface containing repos to access the tables associated with the Game schema
    /// </summary>
    public interface IGameSchema
    {
        /// <inheritdoc cref="IBoardRepo"/>
        IBoardRepo BoardRepo { get; }
        /// <inheritdoc cref="IBoardTileRepo"/>
        IBoardTileRepo BoardTileRepo { get; }
    }

    /// <inheritdoc cref="IGameSchema"/>
    public class GameSchema : BaseSchema, IGameSchema
    {
        private IBoardRepo _boardRepo;
        private IBoardTileRepo _boardTileRepo;

        public GameSchema(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public IBoardRepo BoardRepo => _boardRepo ??= new BoardRepo(base.DataSvc);
        public IBoardTileRepo BoardTileRepo => _boardTileRepo ??= new BoardTileRepo(base.DataSvc);
    }
}
