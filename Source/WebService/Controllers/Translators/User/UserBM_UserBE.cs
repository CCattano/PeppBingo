using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using System;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.User
{
    public class UserBM_UserBE : ITypeConverter<UserBM, UserBE>, ITypeConverter<UserBE, UserBM>
    {
        public UserBM Convert(UserBE source, UserBM destination, ResolutionContext context)
        {
            UserBM result = destination ?? new();
            result.DisplayName = source.DisplayName;
            result.ProfileImageUri = source.ProfileImageUri;
            return result;
        }

        public UserBE Convert(UserBM source, UserBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
