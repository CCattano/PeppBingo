using System;
using System.Collections.Generic;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Caches
{
    public interface IUserSubmitCache
    {
        /// <summary>
        /// Add userID to list of Users who cannot submit
        /// </summary>
        void MarkUserAsUnableToSubmit(int userID);

        /// <summary>
        /// Get whether a user can or cannot submit
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        bool GetCanSubmitForUser(int userID);

        /// <summary>
        /// Clear list of users that could no longer submit 
        /// </summary>
        void ClearCache();
    }

    public class UserSubmitCache : IUserSubmitCache
    {
        private readonly HashSet<int> _usersThatCannotSubmit = new();
        private DateTime _lockUntil = DateTime.Now;
        private readonly object _lock = new();

        public void MarkUserAsUnableToSubmit(int userID)
        {
            lock (_lock)
            {
                if (DateTime.Now > _lockUntil)
                    _usersThatCannotSubmit.Add(userID);
            }
        }

        public bool GetCanSubmitForUser(int userID)
        {
            bool userCannotSubmit = false;
            lock (_lock)
            {
                if (DateTime.Now > _lockUntil)
                    userCannotSubmit = _usersThatCannotSubmit.Contains(userID);
            }

            return userCannotSubmit;
        }

        public void ClearCache()
        {
            lock (_lock)
            {
                if (DateTime.Now <= _lockUntil) return;
                _lockUntil = DateTime.Now.AddSeconds(30);
                _usersThatCannotSubmit.Clear();
            }
        }
    }
}