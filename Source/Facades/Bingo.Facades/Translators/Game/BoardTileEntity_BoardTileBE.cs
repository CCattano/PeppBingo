using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.Data.Entities.Game;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Game
{
    public class BoardTileEntity_BoardTileBE : ITypeConverter<BoardTileEntity, BoardTileBE>, ITypeConverter<BoardTileBE, BoardTileEntity>
    {
        public BoardTileEntity Convert(BoardTileBE source, BoardTileEntity destination, ResolutionContext context)
        {
            BoardTileEntity result = destination ?? new();
            if (result.TileID == default)
                result.TileID = source.TileID;
            // Passed up and used in Create only, not used by Update
            result.BoardID = source.BoardID;
            result.Text = source.Text;
            result.IsFreeSpace = source.IsFreeSpace;
            result.IsActive = source.IsActive;
            //result.CreatedDateTime = source.CreatedDateTime;
            // Passed up and used in Create only, not used by Update
            result.CreatedBy = source.CreatedBy;
            //result.ModDateTime = source.ModDateTime;
            result.ModBy = source.ModBy;
            return result;
        }

        public BoardTileBE Convert(BoardTileEntity source, BoardTileBE destination, ResolutionContext context)
        {
            BoardTileBE result = destination ?? new();
            result.TileID = source.TileID;
            result.BoardID = source.BoardID;
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
