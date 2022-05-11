using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.ApproveSubmissionEvent;
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
        Task LatestActiveBoardID(int activeBoardID);

        /// <summary>
        /// Emits an event containing the bingo board data
        /// of a user who has achieved a bingo
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        Task BingoSubmission(BingoSubmissionEvent submission);

        /// <summary>
        /// Emits a void event to all connected clients that a user who
        /// submitted a bingo for voting has canceled their request
        /// </summary>
        /// <param name="hubConnectionID"></param>
        /// <returns></returns>
        Task CancelSubmission(string hubConnectionID);

        /// <summary>
        /// Emits an event to the user who submitted a bingo
        /// signifying that another user approved their bingo
        /// </summary>
        /// <param name="evtData"></param>
        /// <returns></returns>
        Task ApproveSubmission(ApproveSubmissionEvent evtData);

        /// <summary>
        /// Emits an event to the user who submitted a bingo
        /// signifying that another user rejected their bingo
        /// </summary>
        /// <returns></returns>
        Task RejectSubmission();
        /// <summary>
        /// Emits an event to a connected user to
        /// reset the board they are playing on
        /// </summary>
        /// <returns></returns>
        Task ResetBoard();
    }

    public class BasePlayerHub : Hub<IBasePlayerHub>
    {
    }
}