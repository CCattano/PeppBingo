using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Hubs
{
    public interface IPlayerHub
    {
        /// <summary>
        /// Emits an event containing the ID of the
        /// new board all players should be playing
        /// </summary>
        /// <returns></returns>
        Task EmitLatestActiveBoardID(int activeBoardID);
    }

    public class PlayerHub : Hub<IPlayerHub>
    {
        public async Task EmitLatestActiveBoardID(int activeBoardID) =>
            await base.Clients.All.EmitLatestActiveBoardID(activeBoardID);
    }
}
