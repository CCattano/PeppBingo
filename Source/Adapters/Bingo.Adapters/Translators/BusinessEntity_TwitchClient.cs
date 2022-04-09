using AutoMapper;
using Pepp.Web.Apps.Bingo.Adapters.Translators.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Infrastructure;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;

namespace Pepp.Web.Apps.Bingo.Adapters.Translators
{
    public class BusinessEntity_TwitchClient : Profile
    {
        public BusinessEntity_TwitchClient()
        {
            #region Twitch Client to BE Translators
            this.RegisterTranslator<TwitchAccessToken, AccessTokenBE, TwitchAccessToken_AccessTokenBE>();
            this.RegisterTranslator<TwitchUser, UserBE, TwitchUser_UserBE>();
            #endregion
        }
    }
}
