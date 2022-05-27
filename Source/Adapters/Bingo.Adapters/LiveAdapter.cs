using System;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Hubs.Admin;
using Pepp.Web.Apps.Bingo.Hubs.Player;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;

namespace Pepp.Web.Apps.Bingo.Adapters
{
    public interface ILiveAdapter
    {
        /// <summary>
        /// Updates the ID of the current board users should be playing
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Broadcasts an event that triggers a 30s
        ///         cooldown to set a different active board
        ///     </para>
        ///     <para>
        ///         Broadcasts an event that emits
        ///         the new currently active board
        ///     </para>
        /// </remarks>
        /// <param name="activeBoardID"></param>
        Task SetActiveBoardID(int activeBoardID);

        /// <summary>
        /// Get the last known DateTime a reset event occurred.
        /// </summary>
        /// <returns></returns>
        DateTime? GetResetBoardDateTime();
        
        /// <summary>
        /// Resets all boards for all players
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Broadcasts an event that triggers a
        ///         30s cooldown to reset boards again
        ///     </para>
        ///     <para>
        ///         Broadcasts an event that triggers all
        ///         connected player's boards to reset
        ///         themselves
        ///     </para>
        /// </remarks>
        /// <returns></returns>
        Task ResetAllBoards();
    }

    public class LiveAdapter : ILiveAdapter
    {
        private readonly ILiveDataCache _liveDataCache;
        private readonly IAdminHub _adminHub;
        private readonly IPlayerHub _playerHub;

        public LiveAdapter(
            ILiveDataCache liveDataCache,
            IAdminHub adminHub,
            IPlayerHub playerHub
        )
        {
            _liveDataCache = liveDataCache;
            _adminHub = adminHub;
            _playerHub = playerHub;
        }

        public async Task SetActiveBoardID(int activeBoardID)
        {
            _liveDataCache.SetActiveBoardID(activeBoardID);
            // Trigger cooldown on setting new active board
            await _adminHub.StartSetActiveBoardCooldown();
            // Trigger cooldown on resetting players' board, as they've just been reset by switching
            await _adminHub.StartResetAllBoardsCooldown(_liveDataCache.GetLastResetDateTime()!.Value);
            // For any admins on the live control page broadcast the newest activeBoardID
            await _adminHub.EmitLatestActiveBoardID(activeBoardID);
            // For any players on the play page broadcast the newest activeBoardID
            await _playerHub.EmitLatestActiveBoardID(activeBoardID);
        }

        public DateTime? GetResetBoardDateTime() => _liveDataCache.GetLastResetDateTime();

        public async Task ResetAllBoards()
        {
            // Clear list of users who cannot submit for bingo
            _liveDataCache.ResetUserCanSubmitCache(LiveDataCache.ResetSource.BoardReset);
            // Start 30s cooldown for admins on client so Reset btn cannot be mashed
            await _adminHub.StartResetAllBoardsCooldown(_liveDataCache.GetLastResetDateTime()!.Value);
            // Reset all player's boards
            await _playerHub.ResetBoard(_liveDataCache.GetResetEventID());
        }
    }
}