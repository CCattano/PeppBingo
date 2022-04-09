using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;
using System;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Twitch
{
    public class AccessTokenEntity_AccessTokenBE : ITypeConverter<AccessTokenEntity, AccessTokenBE>, ITypeConverter<AccessTokenBE, AccessTokenEntity>
    {
        public AccessTokenEntity Convert(AccessTokenBE source, AccessTokenEntity destination, ResolutionContext context)
        {
            destination ??= new();
            destination.TwitchUserID = source.TwitchUserID;
            destination.Token = source.AccessToken;
            destination.RefreshToken = source.RefreshToken;
            return destination;
        }

        public AccessTokenBE Convert(AccessTokenEntity source, AccessTokenBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
