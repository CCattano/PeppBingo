using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Hubs.Admin;
using Pepp.Web.Apps.Bingo.Hubs.Player;
using Pepp.Web.Apps.Bingo.Infrastructure.Managers;

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
        ///         Broadcasts and event that emits
        ///         the new currently active board
        ///     </para>
        /// </remarks>
        /// <param name="activeBoardID"></param>
        Task SetActiveBoardID(int activeBoardID);
    }

    public class LiveAdapter : ILiveAdapter
    {
        private readonly ILiveControlsManager _manager;
        private readonly IAdminHub _adminHub;
        private readonly IPlayerHub _playerHub;

        public LiveAdapter(
            ILiveControlsManager manager,
            IAdminHub adminHub,
            IPlayerHub playerHub)
        {
            _manager = manager;
            _adminHub = adminHub;
            _playerHub = playerHub;
        }

        public async Task SetActiveBoardID(int activeBoardID)
        {
            _manager.SetActiveBoardID(activeBoardID);
            // Trigger cooldown on setting new active board
            await _adminHub.TriggerSetActiveBoardCooldown();
            // For any admins on the live control page broadcast the newest activeBoardID
            await _adminHub.EmitLatestActiveBoardID(activeBoardID);
            // For any players on the play page broadcast the newest activeBoardID
            await _playerHub.EmitLatestActiveBoardID(activeBoardID);
        }
    }
}