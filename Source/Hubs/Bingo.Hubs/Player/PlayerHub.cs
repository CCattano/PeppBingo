using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.ApproveSubmissionEvent;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission;

namespace Pepp.Web.Apps.Bingo.Hubs.Player
{
    public interface IPlayerHub
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

        /// <summary>
        /// Emits a void event to all connected clients that a user who
        /// submitted a bingo for voting has canceled their request
        /// </summary>
        /// <param name="hubConnID"></param>
        /// <returns></returns>
        Task EmitSubmissionCancel(string hubConnID);
        
        /// <summary>
        /// Emits an event to the user who submitted a bingo
        /// signifying that another user approved their bingo
        /// </summary>
        /// <param name="requestorHubConnID"></param>
        /// <param name="evtDetails"></param>
        /// <returns></returns>
        Task EmitApproveSubmission(string requestorHubConnID, ApproveSubmissionEvent evtDetails);
        
        /// <summary>
        /// Emits an event to the user who submitted a bingo
        /// signifying that another user rejected their bingo
        /// </summary>
        /// <param name="requestorHubConnID"></param>
        /// <returns></returns>
        Task EmitRejectSubmission(string requestorHubConnID);
        /// <summary>
        /// Emits an event to a connected user to
        /// reset the board they are playing on
        /// </summary>
        /// <param name="resetEventID">
        /// A unique guid-format string that represents this reset event that is firing
        /// </param>
        /// <returns></returns>
        Task ResetBoard(string resetEventID);
    }

    public class PlayerHub : IPlayerHub
    {
        private readonly IHubContext<BasePlayerHub, IBasePlayerHub> _playerHub;

        public PlayerHub(IHubContext<BasePlayerHub, IBasePlayerHub> playerHub) =>
            _playerHub = playerHub;

        public async Task EmitLatestActiveBoardID(int activeBoardID) =>
            await _playerHub.Clients.All.LatestActiveBoardID(activeBoardID);

        public async Task EmitBingoSubmission(BingoSubmissionEvent submission) =>
            await _playerHub.Clients.AllExcept(submission.SubmitterConnectionID).BingoSubmission(submission);

        public async Task EmitSubmissionCancel(string hubConnID) =>
            await _playerHub.Clients.AllExcept(hubConnID).CancelSubmission(hubConnID);
        
        public async Task EmitApproveSubmission(string requestorHubConnID, ApproveSubmissionEvent evtData) =>
            await _playerHub.Clients.Client(requestorHubConnID).ApproveSubmission(evtData);
        
        public async Task EmitRejectSubmission(string requestorHubConnID) =>
            await _playerHub.Clients.Client(requestorHubConnID).RejectSubmission();

        public async Task ResetBoard(string resetEventID) =>
            await _playerHub.Clients.All.ResetBoard(resetEventID);
    }
}