using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;
using System;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.Game
{
    public class BoardBM_BoardBE : ITypeConverter<BoardBM, BoardBE>, ITypeConverter<BoardBE, BoardBM>
    {
        public BoardBM Convert(BoardBE source, BoardBM destination, ResolutionContext context)
        {
            BoardBM result = destination ?? new();
            result.BoardID = source.BoardID;
            result.Name = source.Name;
            result.Description = source.Description;
            result.CreatedDateTime = source.CreatedDateTime;
            result.CreatedBy = source.CreatedBy;
            result.ModDateTime = source.ModDateTime;
            result.ModBy = source.ModBy;
            return result;
        }

        public BoardBE Convert(BoardBM source, BoardBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
