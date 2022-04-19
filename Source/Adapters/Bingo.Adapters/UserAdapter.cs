using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Managers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    public interface IUserAdapter
    {
        /// <summary>
        /// Fetches a user by the JSON web token provided
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserBE> GetUser(string jwt);
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
        /// Sets a user's IsAdmin property to the value provided
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task SetAdminPermissionForUser(int userID, bool isAdmin);
    }

    /// <inheritdoc cref="IUserAdapter"/>
    public class UserAdapter : IUserAdapter
    {
        private readonly IUserFacade _facade;

        public UserAdapter(IUserFacade facade) => _facade = facade;

        public async Task<UserBE> GetUser(string jwt)
        {
            int userID = TokenManager.GetUserIDFromToken(jwt);
            UserBE user = await _facade.GetUser(userID);
            if (user == null)
                throw new WebException(HttpStatusCode.NotFound, "User not found for ID provided");
            return user;
        }

        public async Task<List<UserBE>> GetUsers(string displayName)
        {
            List<UserBE> userBEs = await _facade.GetUsers(displayName);
            return userBEs;
        }

        public async Task<List<UserBE>> GetAdminUsers()
        {
            List<UserBE> userBEs = await _facade.GetAdminUsers();
            return userBEs;
        }

        public async Task SetAdminPermissionForUser(int userID, bool isAdmin)
        {
            await _facade.SetIsAdminForUser(userID, isAdmin);
        }
    }
}
