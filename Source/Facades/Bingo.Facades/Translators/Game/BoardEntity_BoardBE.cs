﻿using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.Data.Entities.Game;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Game
{
    public class BoardEntity_BoardBE : ITypeConverter<BoardEntity, BoardBE>, ITypeConverter<BoardBE, BoardEntity>
    {
        public BoardEntity Convert(BoardBE source, BoardEntity destination, ResolutionContext context)
        {
            BoardEntity result = destination ?? new();
            if (result.BoardID == default)
                result.BoardID = source.BoardID;
            result.Name = source.Name;
            result.Description = source.Description;
            //result.TileCount = source.TileCount;
            //result.CreatedDateTime = source.CreatedDateTime;
            // Passed up and used in Create only, not used by Update
            result.CreatedBy = source.CreatedBy;
            //result.ModDateTime = source.ModDateTime;
            result.ModBy = source.ModBy;
            return result;
        }

        public BoardBE Convert(BoardEntity source, BoardBE destination, ResolutionContext context)
        {
            BoardBE result = destination ?? new();
            result.BoardID = source.BoardID;
            result.Name = source.Name;
            result.Description = source.Description;
            result.TileCount = source.TileCount;
            result.CreatedDateTime = source.CreatedDateTime;
            result.CreatedBy = source.CreatedBy;
            result.ModDateTime = source.ModDateTime;
            result.ModBy = source.ModBy;
            return result;
        }
    }
}
