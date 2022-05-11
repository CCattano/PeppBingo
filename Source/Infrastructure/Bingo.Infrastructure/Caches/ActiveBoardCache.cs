using System;
using Pepp.Web.Apps.Bingo.Infrastructure.Exceptions;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Caches
{
    /// <summary>
    /// Manager that handles holding state data related
    /// to real time settings within the application
    /// </summary>
    public interface IActiveBoardCache
    {
        /// <summary>
        /// Get the ID of the board that
        /// all users should be playing
        /// </summary>
        /// <returns></returns>
        int? GetActiveBoardID();
        /// <summary>
        /// Update the ID of the board
        /// that all users should be playing
        /// </summary>
        /// <param name="activeBoardID"></param>
        void SetActiveBoardID(int activeBoardID);
    }

    public class ActiveBoardCache : IActiveBoardCache
    {
        private int? _activeBoardID;
        private DateTime _lockUntil = DateTime.Now;
        private readonly object _lock = new();
        public int? GetActiveBoardID()
        {
            int? activeBoardID;
            lock (_lock)
            {
                activeBoardID = _activeBoardID;
            }
            return activeBoardID;
        }

        public void SetActiveBoardID(int activeBoardID)
        {
            bool throwEx = false;
            lock (_lock)
            {
                if (DateTime.Now >= _lockUntil)
                {
                    _lockUntil = DateTime.Now.AddSeconds(30);
                    _activeBoardID = activeBoardID;
                }
                else
                {
                    throwEx = true;
                }

            }
            if (throwEx)
                throw new WebException(System.Net.HttpStatusCode.Locked, "ActiveBoardID cannot be modified at this time");
        }
    }
}
