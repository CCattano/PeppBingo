using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using Pepp.Web.Apps.Bingo.Infrastructure;
using Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.Game;
using Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.User;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers.Translators
{
    public class BusinessModels_BusinessEntities : Profile
    {
        public BusinessModels_BusinessEntities()
        {
            this.RegisterTranslator<UserBM, UserBE, UserBM_UserBE>();
            this.RegisterTranslator<BoardBM, BoardBE, BoardBM_BoardBE>();
            this.RegisterTranslator<BoardTileBM, BoardTileBE, BoardTileBM_BoardTileBE>();
            this.RegisterTranslator<GameTileBM, BoardTileBE, GameTileBM_BoardTileBE>();
        }
    }
}
