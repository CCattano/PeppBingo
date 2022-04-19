using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.User;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

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
        /// <summary>
        /// Fetches a User for the <paramref name="userID"/> provided
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserBE> GetUser(int userID);
        /// <summary>
        /// Fetches Users who's <see cref="UserBE.DisplayName"/> contains the <paramref name="displayName"/> provided
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="displayName"/> provided is treated as a substring and any users who's
        ///         <see cref="UserBE.DisplayName"/> contains the <paramref name="displayName"/> will be returned
        ///     </para>
        ///     <para>
        ///         Given a <paramref name="displayName"/> of "sti"
        ///         Then users, "Christian", "Stirling", and "Dustin"
        ///         could be returned as all contain, "sti"
        ///     </para>
        /// </remarks>
        /// <example>
        ///     Given a <paramref name="displayName"/> of "sti"
        ///     Then users, "Christian", "Stirling", and "Dustin"
        ///     could be returned as all contain, "sti"
        /// </example>
        /// <param name="displayName"></param>
        /// <returns></returns>
        Task<List<UserBE>> GetUsers(string displayName);
        /// <summary>
        /// Fetch all users with an IsAdmin value of 1
        /// </summary>
        /// <returns></returns>
        Task<List<UserBE>> GetAdminUsers();
        /// <summary>
        /// Sets a user's IsAdmin field with the value provided
        /// </summary>
        /// <param name="userBE"></param>
        /// <returns></returns>
        Task SetIsAdminForUser(int userID, bool isAdmin);
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

        public async Task<UserBE> GetUser(int userID)
        {
            UserEntity userEntity = await _dataSvc.User.UserRepo.GetUser(userID);
            UserBE userBE = userEntity != null ? _mapper.Map<UserBE>(userEntity) : null;
            return userBE;
        }

        public async Task<List<UserBE>> GetUsers(string displayName)
        {
            List<UserEntity> userEntities =
                await _dataSvc.User.UserRepo.GetUsers(displayName);
            List<UserBE> userBEs =
                userEntities?.Select(user => _mapper.Map<UserBE>(user)).ToList();
            return userBEs;
        }

        public async Task<List<UserBE>> GetAdminUsers()
        {
            List<UserEntity> userEntities =
                await _dataSvc.User.UserRepo.GetAdminUsers();
            List<UserBE> userBEs =
                userEntities?.Select(user => _mapper.Map<UserBE>(user)).ToList();
            return userBEs;
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
            /*
             * Map BE values onto Entity from Db to update entity contents
             *
             * In any other case we want to assume the state of the userBE
             * is asbsolute due to a process that occurred in the UI
             *
             * But in this outlying edge case our UserBE has IsAdmin set
             * to false due to translating from a TwitchUser to a UserBE
             *
             * And in this one case we do NOT want to apply that
             * IsAdmin=False to our UserEntity when updating it
             * So we're taking the server's value and putting it on the BE
             *
             * This way when the BE maps to the DE, the BE will
             * be placing the server's own value onto the DE
             */
            userBE.IsAdmin = userEntity.IsAdmin;
            userEntity = _mapper.Map(userBE, userEntity);
            // Post latest contents to Db
            await _dataSvc.User.UserRepo.UpdateUser(userEntity);
            UserBE updatedUser = _mapper.Map<UserBE>(userEntity);
            return updatedUser;
        }

        public async Task SetIsAdminForUser(int userID, bool isAdmin)
        {
            UserEntity userEntity = await _dataSvc.User.UserRepo.GetUser(userID);
            if (userEntity == null)
                throw new WebException(HttpStatusCode.BadRequest, "Could not update user");
            userEntity.IsAdmin = isAdmin;
            await _dataSvc.User.UserRepo.UpdateUser(userEntity);
        }
    }
}
