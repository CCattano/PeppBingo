using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Pepp.Web.Apps.Bingo.Hubs.Admin
{
    public interface IBaseAdminHub
    {
        /// <summary>
        /// Emits an event that causes the UI to deactivate the
        /// ability to set an active board for 30 seconds
        /// </summary>
        /// <returns></returns>
        Task StartSetActiveBoardCooldown();

        /// <summary>
        /// Emits an event that contains the latest active
        /// board ID Used by the UI to display the latest
        /// selected board on the LiveControls page
        /// </summary>
        /// <returns></returns>
        Task LatestActiveBoardID(int activeBoardID);

        /// <summary>
        /// Emits an event that causes the UI to deactivate the
        /// ability to reset all player's boards for 30 seconds
        /// </summary>
        /// <returns></returns>
        Task StartResetAllBoardsCooldown(DateTime resetDateTime);
    }

    public class BaseAdminHub : Hub<IBaseAdminHub>
    {
    }
}