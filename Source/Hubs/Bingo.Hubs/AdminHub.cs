using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Hubs
{
    public interface IAdminHub
    {
        /// <summary>
        /// Emits an event that causes the UI to deactivate the
        /// ability to set an active board for 30 seconds
        /// </summary>
        /// <returns></returns>
        Task TriggerSetActiveBoardCooldown();
        /// <summary>
        /// Emiots an event that contains the latest active
        /// board ID Used by the UI to display the latest
        /// selected board on the LiveControls page
        /// </summary>
        /// <returns></returns>
        Task EmitLatestActiveBoardID(int activeBoardID);
    }

    public class AdminHub : Hub<IAdminHub>
    {
        public async Task TriggerSetActiveBoardCooldown() =>
            await base.Clients.All.TriggerSetActiveBoardCooldown();


        public async Task EmitLatestActiveBoardID(int activeBoardID) =>
            await base.Clients.All.EmitLatestActiveBoardID(activeBoardID);
    }
}
