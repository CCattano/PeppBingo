using Pepp.Web.Apps.Bingo.Infrastructure.Exceptions;
using System;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Managers
{
    /// <summary>
    /// Manager that handles holding state data related
    /// to real time settings within the application
    /// </summary>
    public interface ILiveControlsManager
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

    public class LiveControlsManager : ILiveControlsManager
    {
        private int? ActiveBoardID;
        private DateTime LockUntil = DateTime.Now;
        private readonly object LOCK = new();
        public int? GetActiveBoardID()
        {
            int? activeBoardID;
            lock (LOCK)
            {
                activeBoardID = ActiveBoardID;
            }
            return activeBoardID;
        }

        public void SetActiveBoardID(int activeBoardID)
        {
            bool throwEx = false;
            lock (LOCK)
            {
                if (DateTime.Now >= LockUntil)
                {
                    LockUntil = DateTime.Now.AddSeconds(30);
                    ActiveBoardID = activeBoardID;
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
