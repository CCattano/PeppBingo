﻿using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Facades;
using Pepp.Web.Apps.Bingo.Managers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;
using Pepp.Web.Apps.Bingo.Infrastructure.Enums;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    public interface IUserAdapter
    {
        /// <summary>
        /// Fetches a user by the JSON web token provided
        /// </summary>
        /// <param name="jwt"></param>
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
        /// Fetch Users who's UserID is in the provided list
        /// </summary>
        /// <returns></returns>
        Task<List<UserBE>> GetUsers(List<int> userIDs);

        /// <summary>
        /// Fetch all users with an IsAdmin value of 1
        /// </summary>
        /// <returns></returns>
        Task<List<UserBE>> GetAdminUsers();

        /// <summary>
        /// Sets a user's IsAdmin property to the value provided
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        Task SetAdminPermissionForUser(int userID, bool isAdmin);
        /// <summary>
        /// Logs that a user performed a
        /// suspicious action in the client
        /// </summary>
        /// <param name="userID"></param>
        void LogSuspiciousUserBehaviour(int userID);
        /// <summary>
        /// Add userID to list of Users who cannot submit
        /// </summary>
        /// <param name="userID"></param>
        void MarkUserAsBingoSubmitted(int userID);
        /// <summary>
        /// Get whether a user can or cannot submit a bingo
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        UserSubmissionStatus GetUserSubmissionStatus(int userID);
    }

    /// <inheritdoc cref="IUserAdapter"/>
    public class UserAdapter : IUserAdapter
    {
        private readonly IUserFacade _facade;
        private readonly ILiveDataCache _liveDataCache;

        public UserAdapter(IUserFacade facade, ILiveDataCache liveDataCache)
        {
            _facade = facade;
            _liveDataCache = liveDataCache;
        }

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

        public async Task<List<UserBE>> GetUsers(List<int> userIDs)
        {
            List<UserBE> userBEs = await _facade.GetUsers(userIDs);
            if ((userBEs?.Count ?? 0) != (userIDs?.Count ?? 0))
                throw new WebException(HttpStatusCode.BadRequest, "Not all users were found for the IDs provided");
            return userBEs;
        }

        public async Task SetAdminPermissionForUser(int userID, bool isAdmin)
        {
            await _facade.SetIsAdminForUser(userID, isAdmin);
        }

        public void LogSuspiciousUserBehaviour(int userID) =>
            _liveDataCache.LogSuspiciousBehaviourForUser(userID);

        public void MarkUserAsBingoSubmitted(int userID) =>
            _liveDataCache.MarkUserAsBingoSubmitted(userID);
        
        public UserSubmissionStatus GetUserSubmissionStatus(int userID) =>
            _liveDataCache.GetUserSubmissionStatus(userID);
    }
}