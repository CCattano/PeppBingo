using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data.Entities.User;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.User
{
    public class UserEntity_UserBE : ITypeConverter<UserEntity, UserBE>, ITypeConverter<UserBE, UserEntity>
    {
        public UserEntity Convert(UserBE source, UserEntity destination, ResolutionContext context)
        {
            UserEntity result = destination ?? new();
            // If a userID exists on an instance of UserEntity provided
            // to this translator then we will keep that UserID
            if (result.UserID == default)
                result.UserID = source.UserID;
            result.TwitchUserID = source.TwitchUserID;
            result.DisplayName = source.DisplayName;
            result.ProfileImageUri = source.ProfileImageUri;
            result.IsAdmin = source.IsAdmin;
            return result;
        }

        public UserBE Convert(UserEntity source, UserBE destination, ResolutionContext context)
        {
            UserBE result = destination ?? new();
            result.UserID = source.UserID;
            result.TwitchUserID = source.TwitchUserID;
            result.DisplayName = source.DisplayName;
            result.ProfileImageUri = source.ProfileImageUri;
            result.IsAdmin = source.IsAdmin;
            return result;
        }
    }
}
