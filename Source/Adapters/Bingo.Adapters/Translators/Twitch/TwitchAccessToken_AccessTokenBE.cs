using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;

namespace Pepp.Web.Apps.Bingo.Adapters.Translators.Twitch
{
    public class TwitchAccessToken_AccessTokenBE : ITypeConverter<AccessTokenBE, TwitchAccessToken>, ITypeConverter<TwitchAccessToken, AccessTokenBE>
    {
        public AccessTokenBE Convert(TwitchAccessToken source, AccessTokenBE destination, ResolutionContext context)
        {
            AccessTokenBE result = destination ?? new();
            result.AccessToken = source.AccessToken;
            result.RefreshToken = source.RefreshToken;
            return result;
        }

        public TwitchAccessToken Convert(AccessTokenBE source, TwitchAccessToken destination, ResolutionContext context)
        {
            TwitchAccessToken result = new()
            {
                AccessToken = source.AccessToken,
                RefreshToken = source.RefreshToken
            };
            return result;
        }
    }
}
