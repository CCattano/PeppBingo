using System;
using System.Collections.Generic;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Caches
{
    public interface IUserCanSubmitCache
    {
        /// <summary>
        /// A unique guid-format string that represents this reset event that is firing
        /// </summary>
        string ResetEventID { get; }

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
        void ResetUserCanSubmitCache();

        /// <summary>
        /// Logs that a user performed a
        /// suspicious action in the client
        /// </summary>
        /// <param name="userID"></param>
        void LogSuspiciousBehaviourForUser(int userID);
    }

    public class UserCanSubmitCache : IUserCanSubmitCache
    {
        public string ResetEventID { get; private set; } = Guid.NewGuid().ToString();
        private DateTime _lastResetDateTime = DateTime.UtcNow;
        private readonly HashSet<int> _usersThatCannotSubmit = new();
        private DateTime _lockCacheResetUntil = DateTime.Now;
        private readonly object _canSubmitLock = new();

        private readonly Dictionary<int, List<DateTime>> _suspiciousBehaviourByUserID = new();
        private readonly object _suspiciousBehaviourLock = new();

        public void LogSuspiciousBehaviourForUser(int userID)
        {
            lock (_suspiciousBehaviourLock)
            {
                if (!_suspiciousBehaviourByUserID.TryGetValue(userID, out List<DateTime> suspiciousActivityDateTimes))
                {
                    // If this user has no entries in our suspicious behaviour log add their first one
                    // Every user gets one for free, because they're likely signing in for the first time
                    // It's the 2nd, 3rd, and 4th entry that we may penalize them on
                    _suspiciousBehaviourByUserID.Add(userID, new List<DateTime>
                    {
                        DateTime.UtcNow
                    });
                }
                else
                {
                    // User has suspicious activity already logged.
                    // Add this new entry then determine if punitive action is necessary
                    if (suspiciousActivityDateTimes.Count == 4)
                        // This is a sliding range, and if we have our maximum 4 entries
                        // The newest one coming into the list, pushes the oldest one out
                        suspiciousActivityDateTimes.RemoveAt(0);
                    suspiciousActivityDateTimes.Add(DateTime.UtcNow);

                    int minutesSinceLastReset =
                        (int)Math.Round((DateTime.UtcNow - _lastResetDateTime).TotalMinutes);

                    /*
                     * Our arbitrary thresholds for suspicious behaviour are as follows
                     *
                     * 3 violations beyond the first (4 total) between 10 and 19 minutes after a reset event and you cannot submit
                     * 2 violations beyond the first (3 total) between 20 and 29 minutes after a reset event and you cannot submit
                     * 1 violation beyond the first (2 total) 30 minutes or more after a reset event and you cannot submit
                     *
                     * The logic for these violation thresholds is as follow
                     *
                     * Within the first 10 minutes of a reset not enough is likely to happen to actually generate a bingo
                     * Cheat and game the system as much as you can manage I suppose. It's not going to help you any.
                     *
                     * Between 10 and 19 minutes after a reset, a low amount of bingo tile events may happen such that
                     * gaming the system to get your card reset may result in 1 to 3 tiles being markable in a row by force and
                     * not luck giving you an unfair advantage and so you can no longer submit your bingo for the leaderboards.
                     *
                     * Between 20 and 29 minutes after a rest, a low to moderate amount of bingo tile events may happen such that
                     * gaming the system to get your card reset may result in 2 to 4 tiles being markable in a row by force and
                     * not luck giving you an unfair advantage and so you can no longer submit your bingo for the leaderboards.
                     *
                     * Any amount of time 30 minutes after a rest, a moderate to high amount of bingo tile events may happen such
                     * that gaming the system to get your card reset may result in 3 to 5 tiles being markable in a row by force
                     * and not luck giving you an unfair advantage and so you can no longer submit your bingo for the leaderboards.
                     *
                     * The violations logged start counting against you after the first-most.
                     *
                     * i.e. it is not the 1st violation, but the 2nd that would disqualify you 45 minutes after a reset.
                     *
                     * This is b/c you may be legitimately joining in and receiving a fresh bingo card from scratch 45
                     * minutes after a reset event if you have just found the stream and have just started participating.
                     *
                     * So we will in good faith give everyone 1 free pass on what would be marked, "suspicious behaviour"
                     * but all instances of suspicious behaviour after the first between reset events will count against the user.
                     */
                    switch (minutesSinceLastReset)
                    {
                        // User has had 1+3 suspicious behaviour events logged between 10 and 19 minutes after reset
                        // They can no longer submit their bingo for the leaderboards until a reset event occurs 
                        case >= 10 and < 20 when suspiciousActivityDateTimes.Count == 4:
                        // User has had 1+2 suspicious behaviour events logged between 20 and 29 minutes after reset
                        // They can no longer submit their bingo for the leaderboards until a reset event occurs
                        case >= 20 and < 30 when suspiciousActivityDateTimes.Count == 3:
                        // User has had 1+1 suspicious behaviour events logged any time 30 minutes or more after reset
                        // They can no longer submit their bingo for the leaderboards until a reset event occurs
                        case >= 30 when suspiciousActivityDateTimes.Count == 2:
                            MarkUserAsUnableToSubmit(userID);
                            break;
                    }
                }
            }
        }
        
        public void MarkUserAsUnableToSubmit(int userID)
        {
            lock (_canSubmitLock)
            {
                _usersThatCannotSubmit.Add(userID);
            }
        }

        public bool GetCanSubmitForUser(int userID)
        {
            bool userCannotSubmit;
            lock (_canSubmitLock)
            {
                userCannotSubmit = _usersThatCannotSubmit.Contains(userID);
            }

            return userCannotSubmit;
        }

        public void ResetUserCanSubmitCache()
        {
            lock (_suspiciousBehaviourLock)
            {
                lock (_canSubmitLock)
                {
                    if (DateTime.UtcNow <= _lockCacheResetUntil) return;
                    _lockCacheResetUntil = DateTime.UtcNow.AddSeconds(30);
                    _usersThatCannotSubmit.Clear();
                    _suspiciousBehaviourByUserID.Clear();
                    _lastResetDateTime = DateTime.UtcNow;
                    ResetEventID = Guid.NewGuid().ToString();
                }
            }
        }
    }
}