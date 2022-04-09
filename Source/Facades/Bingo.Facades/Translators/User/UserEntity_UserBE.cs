using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data.Entities.User;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.User
{
    public class UserEntity_UserBE : ITypeConverter<UserEntity, UserBE>, ITypeConverter<UserBE, UserEntity>
    {
        public UserEntity Convert(UserBE source, UserEntity destination, ResolutionContext context)
        {
            destination ??= new();
            // If a userID exists on an instance of UserEntity provided
            // to this translator then we will keep that UserID
            if (destination.UserID == default)
                destination.UserID = source.UserID;
            destination.TwitchUserID = source.TwitchUserID;
            destination.DisplayName = source.DisplayName;
            destination.ProfileImageUri = source.ProfileImageUri;
            return destination;
        }

        public UserBE Convert(UserEntity source, UserBE destination, ResolutionContext context)
        {
            destination ??= new();
            destination.UserID = source.UserID;
            destination.TwitchUserID = source.TwitchUserID;
            destination.DisplayName = source.DisplayName;
            destination.ProfileImageUri = source.ProfileImageUri;
            return destination;
        }
    }
}
