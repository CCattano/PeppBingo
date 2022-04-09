using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using System;

namespace Pepp.Web.Apps.Bingo.Adapters.Translators.Twitch
{
    public class TwitchAccessToken_AccessTokenBE : ITypeConverter<AccessTokenBE, TwitchAccessToken>, ITypeConverter<TwitchAccessToken, AccessTokenBE>
    {
        public AccessTokenBE Convert(TwitchAccessToken source, AccessTokenBE destination, ResolutionContext context)
        {
            destination ??= new AccessTokenBE();
            destination.AccessToken = source.AccessToken;
            destination.RefreshToken = source.RefreshToken;
            return destination;
        }

        public TwitchAccessToken Convert(AccessTokenBE source, TwitchAccessToken destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
