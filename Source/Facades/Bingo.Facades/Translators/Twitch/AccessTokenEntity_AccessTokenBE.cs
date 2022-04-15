using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Twitch
{
    public class AccessTokenEntity_AccessTokenBE : ITypeConverter<AccessTokenEntity, AccessTokenBE>, ITypeConverter<AccessTokenBE, AccessTokenEntity>
    {
        public AccessTokenEntity Convert(AccessTokenBE source, AccessTokenEntity destination, ResolutionContext context)
        {
            AccessTokenEntity result = destination ?? new();
            result.TwitchUserID = source.TwitchUserID;
            result.Token = source.AccessToken;
            result.RefreshToken = source.RefreshToken;
            return result;
        }

        public AccessTokenBE Convert(AccessTokenEntity source, AccessTokenBE destination, ResolutionContext context)
        {
            AccessTokenBE result = new(source.TwitchUserID);
            result.AccessToken = source.Token;
            result.RefreshToken = source.RefreshToken;
            return result;
        }
    }
}
