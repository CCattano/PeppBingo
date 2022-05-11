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
        private readonly IActiveBoardCache _activeBoardCache;
        private readonly IUserSubmitCache _userSubmitCache;
        private readonly IAdminHub _adminHub;
        private readonly IPlayerHub _playerHub;

        public LiveAdapter(
            IActiveBoardCache activeBoardCache,
            IUserSubmitCache userSubmitCache,
            IAdminHub adminHub,
            IPlayerHub playerHub
        )
        {
            _activeBoardCache = activeBoardCache;
            _userSubmitCache = userSubmitCache;
            _adminHub = adminHub;
            _playerHub = playerHub;
        }

        public async Task SetActiveBoardID(int activeBoardID)
        {
            _activeBoardCache.SetActiveBoardID(activeBoardID);
            // Trigger cooldown on setting new active board
            await _adminHub.StartSetActiveBoardCooldown();
            // For any admins on the live control page broadcast the newest activeBoardID
            await _adminHub.LatestActiveBoardID(activeBoardID);
            // For any players on the play page broadcast the newest activeBoardID
            await _playerHub.EmitLatestActiveBoardID(activeBoardID);
        }

        public async Task ResetAllBoards()
        {
            // Clear list of users who cannot submit for bingo
            _userSubmitCache.ClearCache();
            // Start 30s cooldown for admins on client so Reset btn cannot be mashed
            await _adminHub.StartResetAllBoardsCooldown();
            // Reset all player's boards
            await _playerHub.ResetBoard();
        }
    }
}