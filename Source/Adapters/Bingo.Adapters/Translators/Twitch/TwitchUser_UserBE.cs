using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;

namespace Pepp.Web.Apps.Bingo.Adapters.Translators.Twitch
{
    public class TwitchUser_UserBE : ITypeConverter<TwitchUser, UserBE>, ITypeConverter<UserBE, TwitchUser>
    {
        public UserBE Convert(TwitchUser source, UserBE destination, ResolutionContext context)
        {
            UserBE result = destination ?? new UserBE();
            result.TwitchUserID = source.UserID;
            result.DisplayName = source.DisplayName;
            result.ProfileImageUri = source.ProfileImageUri;
            return result;
        }

        public TwitchUser Convert(UserBE source, TwitchUser destination, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
