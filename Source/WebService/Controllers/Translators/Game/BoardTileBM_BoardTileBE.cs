using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.Game
{
    public class BoardTileBM_BoardTileBE : ITypeConverter<BoardTileBM, BoardTileBE>, ITypeConverter<BoardTileBE, BoardTileBM>
    {
        public BoardTileBM Convert(BoardTileBE source, BoardTileBM destination, ResolutionContext context)
        {
            BoardTileBM result = destination ?? new();
            result.TileID = source.TileID;
            result.Text = source.Text;
            result.IsFreeSpace = source.IsFreeSpace;
            result.IsActive = source.IsActive;
            result.CreatedDateTime = source.CreatedDateTime;
            result.CreatedBy = source.CreatedBy;
            result.ModDateTime = source.ModDateTime;
            result.ModBy = source.ModBy;
            return result;
        }

        public BoardTileBE Convert(BoardTileBM source, BoardTileBE destination, ResolutionContext context)
        {
            BoardTileBE result = destination ?? new();
            result.TileID = source.TileID;
            result.Text = source.Text;
            result.IsFreeSpace = source.IsFreeSpace;
            result.IsActive = source.IsActive;
            result.CreatedDateTime = source.CreatedDateTime;
            result.CreatedBy = source.CreatedBy;
            result.ModDateTime = source.ModDateTime;
            result.ModBy = source.ModBy;
            return result;
        }
    }
}
