using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.Game
{
    public class GameTileBM_BoardTileBE : ITypeConverter<GameTileBM, BoardTileBE>, ITypeConverter<BoardTileBE, GameTileBM>
    {
        public GameTileBM Convert(BoardTileBE source, GameTileBM destination, ResolutionContext context)
        {
            GameTileBM result = destination ?? new();
            result.Text = source.Text;
            result.IsFreeSpace = source.IsFreeSpace;
            return result;
        }

        public BoardTileBE Convert(GameTileBM source, BoardTileBE destination, ResolutionContext context)
        {
            BoardTileBE result = destination ?? new();
            result.Text = source.Text;
            result.IsFreeSpace = source.IsFreeSpace;
            return result;
        }
    }
}
