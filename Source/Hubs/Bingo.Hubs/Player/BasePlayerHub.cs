using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission;

namespace Pepp.Web.Apps.Bingo.Hubs.Player
{
    public interface IBasePlayerHub
    {
        /// <summary>
        /// Emits an event containing the ID of the
        /// new board all players should be playing
        /// </summary>
        /// <returns></returns>
        Task EmitLatestActiveBoardID(int activeBoardID);

        /// <summary>
        /// Emits an event containing the bingo board data
        /// of a user who has achieved a bingo
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        Task EmitBingoSubmission(BingoSubmissionEvent submission);
    }

    public class BasePlayerHub : Hub<IBasePlayerHub>
    {
    }
}