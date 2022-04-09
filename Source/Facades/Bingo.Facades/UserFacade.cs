using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.User;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Facades
{
    /// <summary>
    /// Facade for working with the information
    /// we store that is explicitly related to Users
    /// </summary>
    public interface IUserFacade
    {
        /// <summary>
        /// Updates an existing user if they
        /// exist otherwise inserts the user
        /// </summary>
        /// <param name="userBE"></param>
        /// <returns></returns>
        Task<UserBE> PersistUser(UserBE userBE);
    }

    /// <inheritdoc cref="IUserFacade"/>
    public class UserFacade : IUserFacade
    {
        private readonly IMapper _mapper;
        private readonly IBingoDataService _dataSvc;

        public UserFacade(IMapper mapper, IBingoDataService dataSvc)
        {
            _mapper = mapper;
            _dataSvc = dataSvc;
        }

        public async Task<UserBE> PersistUser(UserBE userBE)
        {
            // Look for existing user by TwitchUserID
            UserEntity userEntity = await _dataSvc.User.UserRepo.GetUser(userBE.TwitchUserID);
            if (userEntity == null)
            {
                // Token does not exist, make new entity from BE data
                userEntity = _mapper.Map<UserEntity>(userBE);
                // Insert BE data into Db
                await _dataSvc.User.UserRepo.InsertUser(userEntity);
                UserBE newUser = _mapper.Map<UserBE>(userEntity);
                return newUser;
            }
            // Map BE values onto Entity from Db to update entity contents
            userEntity = _mapper.Map(userBE, userEntity);
            // Post latest contents to Db
            await _dataSvc.User.UserRepo.UpdateUser(userEntity);
            UserBE updatedUser = _mapper.Map<UserBE>(userEntity);
            return updatedUser;
        }
    }
}
