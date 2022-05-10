using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using Pepp.Web.Apps.Bingo.Data.Entities.Game;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;
using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;
using Pepp.Web.Apps.Bingo.Data.Entities.User;
using Pepp.Web.Apps.Bingo.Facades.Translators.Game;
using Pepp.Web.Apps.Bingo.Facades.Translators.Stats;
using Pepp.Web.Apps.Bingo.Facades.Translators.Twitch;
using Pepp.Web.Apps.Bingo.Facades.Translators.User;
using Pepp.Web.Apps.Bingo.Infrastructure;

namespace Pepp.Web.Apps.Bingo.Facades.Translators
{
    public class Entity_BusinessEntity : Profile
    {
        public Entity_BusinessEntity()
        {
            #region Twitch Translators
            this.RegisterTranslator<SecretEntity, ApiSecretBE, SecretEntity_ApiSecretBE>();
            this.RegisterTranslator<AccessTokenEntity, AccessTokenBE, AccessTokenEntity_AccessTokenBE>();
            #endregion

            #region User Translators
            this.RegisterTranslator<UserEntity, UserBE, UserEntity_UserBE>();
            #endregion

            #region Game Translators
            this.RegisterTranslator<BoardEntity, BoardBE, BoardEntity_BoardBE>();
            this.RegisterTranslator<BoardTileEntity, BoardTileBE, BoardTileEntity_BoardTileBE>();
            #endregion

            #region Stats Translators

            this.RegisterTranslator<LeaderboardEntity, LeaderboardBE, LeaderboardEntity_LeaderboardBE>();
            this.RegisterTranslator<LeaderboardPosEntity, LeaderboardPosBE, LeaderboardPosEntity_LeaderboardPosBE>();

            #endregion
        }
    }
}
