using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;

namespace Pepp.Web.Apps.Bingo.Adapters.Translators.Twitch
{
    public class TwitchUser_UserBE : ITypeConverter<TwitchUser, UserBE>, ITypeConverter<UserBE, TwitchUser>
    {
        public UserBE Convert(TwitchUser source, UserBE destination, ResolutionContext context)
        {
            destination ??= new UserBE();
            destination.TwitchUserID = source.UserID;
            destination.DisplayName = source.DisplayName;
            destination.ProfileImageUri = source.ProfileImageUri;
            return destination;
        }

        public TwitchUser Convert(UserBE source, TwitchUser destination, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
