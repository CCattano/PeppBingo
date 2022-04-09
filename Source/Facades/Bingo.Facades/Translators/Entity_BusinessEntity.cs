using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;
using Pepp.Web.Apps.Bingo.Data.Entities.User;
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
            this.RegisterTranslator<ValueDetailDescEntity, ApiSecretValueDetailDescBE, ValueDetailDescEntity_ApiSecretValueDetailDescBE>();
            this.RegisterTranslator<AccessTokenEntity, AccessTokenBE, AccessTokenEntity_AccessTokenBE>();
            #endregion

            #region User Translators
            this.RegisterTranslator<UserEntity, UserBE, UserEntity_UserBE>();
            #endregion
        }
    }
}
